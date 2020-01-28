using Custodial.BoilerPlate.Core;
using Custodial.BoilerPlate.Core.Database;
using Custodial.BoilerPlate.Core.Interfaces;
using Custodial.BoilerPlate.Core.Service_Settings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [ApiController]
    [Route("[controller]")]
    public class ServiceController 
    {
        public IDatabase database { get; set; }
        public IDatabaseObjectFactory factory { get; set; }
        public IServiceSettings settings { get; set; }

        public ServiceController() 
        {
            if ((settings == null) || (settings.databaseSettings == null) || (settings.networkSettings == null))
            {
                settings = new ServiceSettings();
                settings.InitServiceSettingsAsync("ServiceSettings.json", this.GetType().Assembly);
            }
            if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
            {
                MongoDatabase<DataBaseObject> mongoDB = new MongoDatabase<DataBaseObject>(settings.databaseSettings, new BsonDatabaseObjectConverter());
                mongoDB.bsonConverter = new BsonDatabaseObjectConverter();
                database = mongoDB;
            }
            else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MySqlDatabase))
            {
                database = new MySQLDatabase();
            }
            else
            {
                database = new InMemoryDatabase<DataBaseObject>(settings.databaseSettings);
            }
            factory = new DatabaseObjectFactory<DataBaseObject>()
            {
                db = this.database
            };
        }

        [HttpGet]
        public async Task<string> GetAsync([FromBody] DataBaseObject service = default(DataBaseObject))
        {
            if (service == null || service.iD == null)
            {
                return JsonConvert.SerializeObject(await factory.ReadAllAsync());
            }
            else
            {
                foreach (DataBaseObject ms in await factory.ReadFilteredAsync(service))
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
        public async Task<string> PostAsync([FromBody] DataBaseObject service)
        {
            if (service.iD == null)
            {
                return $"Could Not post {service.ToJson()}";
            }
            return (await factory.CreateAsync(service)).ToJson();
        }

        [HttpDelete]
        public async Task<string> DeleteAsync([FromBody] DataBaseObject service)
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
        public async Task<string> PutAsync([FromBody] List<DataBaseObject> serviceList)
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
