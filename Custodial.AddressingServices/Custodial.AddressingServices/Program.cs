using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Custodial.BoilerPlate.Core;
using Custodial.BoilerPlate.Core.Interfaces;
using Custodial.BoilerPlate.Core.Service_Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson.Serialization;
using Serilog;

namespace Custodial.AddressingServices
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File($"Logs\\Log{DateTime.Now.ToString("yyyyMMdd_HHmm")}.txt")
            .CreateLogger();
            Log.Information("Starting Custodial.Addressing.Services");

            BsonClassMap.RegisterClassMap<Microservice>();
            BsonSerializer.RegisterSerializer(typeof(IDatabaseObject),
                BsonSerializer.LookupSerializer<Microservice>());

            string connectionString = $"";
            IServiceSettings settings = new ServiceSettings();
            await settings.InitServiceSettingsAsync("ServiceSettings.json", Assembly.GetExecutingAssembly());
            foreach (var ip in settings.networkSettings.addresses)
            {
                if (!string.IsNullOrEmpty(ip))
                {
                    if (connectionString.Equals(""))
                    {
                        connectionString = $"http://{ip}:{settings.networkSettings.ports.ElementAt(0)}";
                    }
                    else
                    {
                        connectionString = connectionString + $";http://{ip}:{settings.networkSettings.ports.ElementAt(0)}";
                    }
                }
            }
            Log.Information($"Connecting on: {connectionString}");
            Log.Information($"Starting Database Worker Thread");
            CASWorkerThread worker = new CASWorkerThread();
            Thread thread = new Thread(async () => await worker.MainWorkerThreadAsync(15000));
            thread.Start();
            await new WebHostBuilder()
               .UseKestrel()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .UseUrls(connectionString)
               .UseIISIntegration()
               .UseStartup<Startup>()
               .Build()
               .RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

