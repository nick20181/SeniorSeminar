using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Custodial.BoilerPlate.Service_Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Custodial.BoilerPlate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustodialServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public static async Task Main(string[] args)
        {
            string connectionString = $"";
            IServiceSettings settings = new ServiceSettings();
            await settings.InitServiceSettingsAsync();
            foreach (var ip in settings.networkSettings.addresses)
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

            Console.WriteLine($"Starting on addresses{connectionString}");
            IDatabase db = new MongoDatabase()
            {
                settings = settings.databaseSettings
            };
            var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .UseUrls(connectionString)
                    .Build();

            host.Run();
        }
    }
}

