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
using Newtonsoft.Json;

namespace Custodial.Addressing.Service.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AddressingController : Controller
    {
        public IDatabase database;
        public IDatabaseObjectFactory factory;
        public IServiceSettings settings;

        public AddressingController()
        {
            settings = new ServiceSettings();
            settings.InitServiceSettingsAsync();
            if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
            {
                database = new MongoDatabase(settings.databaseSettings);
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
            if (service == null || service.iD == null)
            {
                return JsonConvert.SerializeObject(await factory.ReadAllAsync());
            }
            else
            {
                foreach (Microservice ms in await factory.ReadFilteredAsync(service))
                {
                    if (ms.iD.Equals(service.iD) && ms.serviceName.Equals(service.serviceName) && ms.timeCreated.Equals(service.timeCreated)
                        && ms.shortName.Equals(ms.shortName) && ms.discription.Equals(service.discription))
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

        [HttpPut]
        public async Task<string> PutAsync([FromBody] List<Microservice> serviceList)
        {
            if (serviceList.Count == 2)
            {
                var service = serviceList.ToArray();
                foreach (var ms in await factory.ReadFilteredAsync(service[0]))
                {
                    if (ms.iD.Equals(service[1].iD))
                    {
                        return (await ms.UpdateAsync(service[1], database)).ToJson();
                    }
                }
            }
            return $"Could Not update {JsonConvert.SerializeObject(serviceList)}";
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}