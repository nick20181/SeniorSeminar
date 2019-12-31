using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Custodial.Addressing.Service.Database;
using Custodial.Addressing.Service.Service_Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Custodial.Addressing.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustodialServiceController : Controller
    {
        private IDatabase database;
        private IDatabaseObjectFactory factory;
        private IServiceSettings settings;

        public CustodialServiceController(IServiceSettings settings)
        {
            this.settings = settings;
            if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
            {
                database = new MongoDatabase();
            } else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MySqlDatabase))
            {
                database = new MySQLDatabase();
            } else
            {
                database = new InMemoryDatabase();
            }
            factory = new MicroserviceFactory()
            {
                db = this.database
            };
        }

        [HttpGet()]
        public async Task<HttpResponseMessage> GetAsync([FromBody] Microservice service = null)
        {
            if (service == null)
            {
                string toReturn = "Start of Microservice list;";
                foreach (Microservice ms in await factory.ReadAllAsync())
                {
                    toReturn = $"{toReturn}\n{ms.ToJson()};";
                }
                toReturn = $"{toReturn}\nEnd of Microservice list;";
                return new HttpResponseMessage()
                {
                    Content = new StringContent(toReturn, Encoding.UTF8)
                };
            } else
            {
                foreach (Microservice ms in await factory.ReadFilteredAsync(service))
                {
                    if (ms.iD.Equals(service.iD) && ms.serviceName.Equals(service.serviceName) && ms.settings.networkSettings.Equals(service.settings.networkSettings))
                    {
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(ms.ToJson(), Encoding.UTF8)
                        };
                    }
                }
            }
            if (service == null)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent("Could not retreive any Microservices", Encoding.UTF8)
                };
            }
            else
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent($"Could not find: {service.ToJson()}", Encoding.UTF8)
                };
            }
        }

        [HttpPost()]
        public async Task<HttpResponseMessage> PostAsync([FromBody] Microservice service)
        {

            if (service == null)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent($"Could Not post {service.ToJson()}", Encoding.UTF8)
                };
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent((await factory.CreateAsync(service)).ToJson(), Encoding.UTF8)
            };
        }

        [HttpDelete()]
        public async Task<HttpResponseMessage> DeleteAsync([FromBody] Microservice service)
        {
            if (!(service == null))
            {
                foreach (var ms in await factory.ReadFilteredAsync(service))
                {
                    if (ms.ToJson().Equals(service.ToJson()))
                    {
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent((await ms.DeleteAsync(database)).ToJson(), Encoding.UTF8)
                        };
                    }
                }
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent($"Could Not delete {service.ToJson()}", Encoding.UTF8)
            };
        }

        [HttpPut("{serviceID}")]
        public async Task<HttpResponseMessage> PutAsync(string serviceID, [FromBody] Microservice serviceUpdated)
        {

            if (!(serviceID == null))
            {
                Microservice service = new Microservice()
                {
                    iD = serviceID
                };
                foreach (var ms in await factory.ReadFilteredAsync(service))
                {
                    if (ms.iD.Equals(service.iD))
                    {
                        return new HttpResponseMessage()
                        {
                            Content = new StringContent((await ms.UpdateAsync(serviceUpdated, database)).ToJson(), Encoding.UTF8)
                        };
                    }
                }
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent($"Could Not update {serviceID} to {serviceUpdated.ToJson()}", Encoding.UTF8)
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        public static async Task Main(string[] args)
        {
            if (false) {
                string connectionString = $"";
                IServiceSettings settings = new ServiceSettings();
                await settings.InitServiceSettingsAsync();
                foreach (var ip in settings.networkSettings.addresses)
                {
                    if (!String.IsNullOrEmpty(ip)) {
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
                var host = new WebHostBuilder()
                        .UseKestrel()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseIISIntegration()
                        .UseStartup<Startup>()
                        .UseUrls(connectionString)
                        .Build();

                host.Run();
            } 
            else
            {
                Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
            }
        }
    }
}

