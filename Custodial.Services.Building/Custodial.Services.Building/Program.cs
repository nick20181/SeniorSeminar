using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Custodial.BoilerPlate.Core;
using Custodial.BoilerPlate.Core.Interfaces;
using Custodial.BoilerPlate.Core.Service_Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Custodial.Services.Building
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
            Log.Information("Starting Custodial.Services.Organization.");

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
            CASHttpConnection CasConnection = new CASHttpConnection(settings, Assembly.GetExecutingAssembly(), Log.Logger, "Custodial.Service.Building", "C.S.B", "Tracks building information for a organization");
            CasConnection.Start();
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
