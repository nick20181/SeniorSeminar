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
        [Route("[controller]/{dataFilter}/{data}")]
        public async Task<string> GetAsync(string dataFilter, string data)
        {

            await factory.ReadFilteredAsync(dataFilter, data);
            return "Could Not Get";
        }

        [HttpGet]
        [Route("[controller]/all")]
        public async Task<string> GetAllAsync()
        {
            string toReturn = "";
            foreach (DataBaseObject ms in await factory.ReadAllAsync())
            {
                toReturn = JsonConvert.SerializeObject(await factory.ReadAllAsync());
            }
            if (String.IsNullOrEmpty(toReturn))
            {
                return "Could not find any Orgainzations.";
            }
            return toReturn;
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
        [Route("[controller]/{id}")]
        public async Task<string> DeleteAsync([FromRoute] string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                foreach (var ms in await factory.ReadFilteredAsync("_id", id))
                {
                    if (ms.iD.Equals(id))
                    {
                        return (await ms.DeleteAsync(database)).ToJson();
                    }
                }
            }
            return $"Could Not delete object: {id}";
        }

        [HttpPut]
        [Route("[controller]/{id}")]
        public async Task<string> PutAsync([FromRoute] string orginalId, [FromBody] DataBaseObject updatedService)
        {
                foreach (var dataObject in await factory.ReadFilteredAsync("_id", orginalId))
                {
                    if (dataObject.iD.Equals(orginalId))
                    {
                        return (await dataObject.UpdateAsync(updatedService, database)).ToJson();
                    }
                }
            return $"Could Not update data object with id: {orginalId}";
        }
    }
}
