using Custodial.Service.Organization.Database;
using Custodial.Service.Organization.Interfaces;
using Custodial.Service.Organization.Service_Settings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Custodial.Service.Organization.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrganizationController<databaseObjectType> : IServiceController<databaseObjectType> where databaseObjectType : IDatabaseObject
    {
        public IDatabase database { get; set; }
        public IDatabaseObjectFactory factory { get; set; }
        public IServiceSettings settings { get; set; }

        public OrganizationController()
        {
            settings = new ServiceSettings();
            settings.InitServiceSettingsAsync();
            if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
            {
                database = new MongoDatabase<databaseObjectType>(settings.databaseSettings);
            }
            else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MySqlDatabase))
            {
                database = new MySQLDatabase();
            }
            else
            {
                database = new InMemoryDatabase<databaseObjectType>(settings.databaseSettings);
            }
            factory = new DatabaseObjectFactory<databaseObjectType>()
            {
                db = this.database
            };
        }

        [HttpGet]
        public async Task<string> GetAsync([FromBody] databaseObjectType service = default(databaseObjectType))
        {
            if (service == null || service.iD == null)
            {
                return JsonConvert.SerializeObject(await factory.ReadAllAsync());
            }
            else
            {
                foreach (databaseObjectType ms in await factory.ReadFilteredAsync(service))
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
        public async Task<string> PostAsync([FromBody] databaseObjectType service)
        {
            if (service.iD == null)
            {
                return $"Could Not post {service.ToJson()}";
            }
            return (await factory.CreateAsync(service)).ToJson();
        }

        [HttpDelete]
        public async Task<string> DeleteAsync([FromBody] databaseObjectType service)
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
        public async Task<string> PutAsync([FromBody] List<databaseObjectType> serviceList)
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
    }
}
