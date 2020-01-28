using Custodial.Service.Organization.Controllers;
using Custodial.Service.Organization.Service_Settings;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Service.Organization
{
    public class APIMain
    {
        public static async Task Main(string[] args)
        {
            string connectionString = $"";
            IServiceSettings settings = new ServiceSettings();
            await settings.InitServiceSettingsAsync();
            foreach (var ip in settings.networkSettings.addresses)
            {
                if (!String.IsNullOrEmpty(ip))
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
            await new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls(connectionString)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build()
                .RunAsync();
        }
    }
}
