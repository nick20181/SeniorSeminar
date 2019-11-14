using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Campus.Service.Address.Implementations;
using Campus.Service.Address.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Campus.Service.Address.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[RequireHttpsOrClose]
    public class ServiceAddressController : Controller
    {
        IServiceSettings settings = new ServiceSettings();
        IDatabase database;
        IMicroServiceFactory microServiceFactory;

        public ServiceAddressController()
        {
            settings.intilizeServiceAsync();
            database = new MongoDatabase(settings.databaseSettings);
            microServiceFactory = new MicroServiceFactory()
            {
                database = database
            };

        }

        [HttpGet("{serviceName}")]
        public async Task<string> GetAsync(string serviceName)
        {
            string toReturn = $"";
            foreach (var service in await microServiceFactory.ReadAsync(new MicroService()
            {
                serviceName = serviceName
            }))
            {
                toReturn = toReturn + service.ToJson();
            }
            return toReturn;
        }

        [HttpGet("All")]
        public async Task<string> GetAsync()
        {
            string toReturn = $"";
            foreach (var service in await microServiceFactory.ReadAsync(new MicroService()))
            {
                toReturn = toReturn + service.ToJson();
            }
            return toReturn;
        }

        [HttpPost("Post")]
        public async Task<string> PostAsync([FromBody] MicroService service)
        {
            return (await microServiceFactory.CreateAsync(service)).ToJson();
        }

        [HttpPut("Put/{serviceName}")]
        public async Task<string> PutAsync(string serviceName, [FromBody]MicroService updatedService)
        {
            var temp = (await microServiceFactory.ReadAsync(new MicroService()
            {
                serviceName = serviceName
            }));
            foreach (var service in temp) {
                if (service.serviceName.Equals(serviceName))
                {
                    ((MicroService) service).database = microServiceFactory.database;
                    return (await service.UpdateAsync(updatedService)).ToJson();
                }
            }
            return $"Could not find {serviceName} in database to update.";
        }

        [HttpDelete("Delete/{serviceName}")]
        public async Task<string> DeleteAsync(string serviceName)
        {
            var temp = (await microServiceFactory.ReadAsync(new MicroService()
            {
                serviceName = serviceName
            }));
            foreach (var service in temp)
            {
                ((MicroService)service).database = microServiceFactory.database;
                if (service.serviceName.Equals(serviceName))
                {
                    return (await service.DeleteAsync()).ToJson();
                }
            }
            return $"Could not find {serviceName} in database to delete.";
        }

        public static async Task Main(string[] args)
        {
            string connectionString = $"";
            IServiceSettings settings = new ServiceSettings();
            await settings.intilizeServiceAsync();
            foreach (var ip in settings.networkSettings.addresses)
            {
                if (connectionString.Equals(""))
                {
                    connectionString = $"http://{ip}:{settings.networkSettings.port}";
                }
                else
                {
                    connectionString = connectionString + $";http://{ip}:{settings.networkSettings.port}";
                }
            }

            Console.WriteLine($"Starting on addresses{connectionString}");
            MongoDatabase db = new MongoDatabase(settings.databaseSettings);
            Thread thread = new Thread(()=> db.workerThread());
            thread.Start();
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
    public class RequireHttpsOrCloseAttribute : RequireHttpsAttribute
    {
        protected override void HandleNonHttpsRequest(AuthorizationFilterContext filterContext)
        {
            filterContext.Result = new StatusCodeResult(400);
        }
    }
}