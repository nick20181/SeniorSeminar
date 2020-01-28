using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Custodial.BoilerPlate.Core;
using Custodial.BoilerPlate.Core.Database;
using Custodial.BoilerPlate.Core.Interfaces;
using Custodial.BoilerPlate.Core.Service_Settings;
using Custodial.Service.Organizations.Convertors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Custodial.Service.Organizations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationController : ControllerBase
    {
        public IDatabase database { get; set; }
        public IDatabaseObjectFactory factory { get; set; }
        public IServiceSettings settings { get; set; }

        public OrganizationController()
        {
            if ((settings == null) || (settings.databaseSettings == null) || (settings.networkSettings == null))
            {
                settings = new ServiceSettings();
                settings.InitServiceSettingsAsync("ServiceSettings.json", this.GetType().Assembly);
            }
            if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
            {
                MongoDatabase<Organization> mongoDB= new MongoDatabase<Organization>(settings.databaseSettings, new BsonOrganizationConverter());
                database = mongoDB;
            }
            else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MySqlDatabase))
            {
                database = new MySQLDatabase();
            }
            else
            {
                database = new InMemoryDatabase<Organization>(settings.databaseSettings);
            }
            factory = new DatabaseObjectFactory<Organization>()
            {
                db = this.database
            };
        }

        [HttpGet]
        public async Task<string> GetAsync([FromBody] Organization service = default(Organization))
        {
            if (service == null || service.iD == null)
            {
                return JsonConvert.SerializeObject(await factory.ReadAllAsync());
            }
            else
            {
                foreach (Organization ms in await factory.ReadFilteredAsync(service))
                {
                    if (ms.iD.Equals(service.iD) && ms.timeCreated.Equals(service.timeCreated))
                    {
                        return ms.ToJson();
                    }
                }
            }
            return "";
        }

        [HttpPost]
        public async Task<string> PostAsync([FromBody] Organization service)
        {
            if (service.iD == null)
            {
                return $"Could Not post {service.ToJson()}";
            }
            return (await factory.CreateAsync(service)).ToJson();
        }

        [HttpDelete]
        public async Task<string> DeleteAsync([FromBody] Organization service)
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
        public async Task<string> PutAsync([FromBody] List<Organization> serviceList)
        {
            if (serviceList.Count == 2)
            {
                var service = serviceList.ToArray();
                foreach (var dataObject in await factory.ReadFilteredAsync(service[0]))
                {
                    if (dataObject.iD.Equals(service[1].iD))
                    {
                        return (await dataObject.UpdateAsync(service[1], database)).ToJson();
                    }
                }
            }
            return $"Could Not update {JsonConvert.SerializeObject(serviceList)}";
        }
    }
}