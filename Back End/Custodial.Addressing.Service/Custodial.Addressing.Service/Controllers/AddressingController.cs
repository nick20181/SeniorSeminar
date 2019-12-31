using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Custodial.Addressing.Service.Databases;
using Custodial.Addressing.Service.Interfaces;
using Custodial.Addressing.Service.Service_Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Custodial.Addressing.Service.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AddressingController : Controller
    {
        private IDatabase database;
        private IDatabaseObjectFactory factory;
        private IServiceSettings settings;

        public AddressingController()
        {
            settings = new ServiceSettings();
            if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
            {
                database = new MongoDatabase();
            }
            else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MySqlDatabase))
            {
                database = new MySQLDatabase();
            }
            else
            {
                database = new InMemoryDatabase();
            }
            factory = new MicroserviceFactory()
            {
                db = this.database
            };
        }

        [HttpGet]
        public async Task<string> GetAsync([FromBody] Microservice service = null)
        {
            if (service == null)
            {
                string toReturn = "Start of Microservice list;";
                foreach (Microservice ms in await factory.ReadAllAsync())
                {
                    toReturn = $"{toReturn}\n{ms.ToJson()};";
                }
                toReturn = $"{toReturn}\nEnd of Microservice list;";
                return toReturn;
            }
            else
            {
                foreach (Microservice ms in await factory.ReadFilteredAsync(service))
                {
                    if (ms.iD.Equals(service.iD) && ms.serviceName.Equals(service.serviceName) && ms.settings.networkSettings.Equals(service.settings.networkSettings))
                    {
                        return ms.ToJson();
                    }
                }
            }
            return Microservice.nullMicroService().ToJson();
        }

        [HttpPost]
        public async Task<string> PostAsync([FromBody] Microservice service)
        {
            if (service.iD == null)
            {
                return $"Could Not post {service.ToJson()}";
            }
            return (await factory.CreateAsync(service)).ToJson();
        }

        [HttpDelete]
        public async Task<string> DeleteAsync([FromBody] Microservice service)
        {
            if (!(service == null))
            {
                foreach (var ms in await factory.ReadFilteredAsync(service))
                {
                    if (ms.ToJson().Equals(service.ToJson()))
                    {
                        return (await ms.DeleteAsync(database)).ToJson();
                    }
                }
            }
            return $"Could Not delete {service.ToJson()}";
        }

        [HttpPut("{serviceID}")]
        public async Task<string> PutAsync(string serviceID, [FromBody] Microservice serviceUpdated)
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
                        return (await ms.UpdateAsync(serviceUpdated, database)).ToJson();
                    }
                }
            }
            return $"Could Not update {serviceID} to {serviceUpdated.ToJson()}";
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}