using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Custodial.BoilerPlate.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Custodial.BoilerPlate.Core;
using Custodial.BoilerPlate.Core.Database;
using Custodial.BoilerPlate.Core.Service_Settings;
using Newtonsoft.Json;

namespace Custodial.AddressingServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressingController : ControllerBase
    {
        public IDatabase database { get; set; }
        public IDatabaseObjectFactory factory { get; set; }
        public IServiceSettings settings { get; set; }

        public AddressingController()
        {
            if ((settings == null) || (settings.databaseSettings == null) || (settings.networkSettings == null))
            {
                settings = new ServiceSettings();
                settings.InitServiceSettingsAsync("ServiceSettings.json", this.GetType().Assembly);
            }
            if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
            {
                MongoDatabase<Microservice> mongoDB = new MongoDatabase<Microservice>(settings.databaseSettings, new BsonMicroserviceConverter());
                database = mongoDB;
            }
            else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MySqlDatabase))
            {
                database = new MySQLDatabase();
            }
            else
            {
                database = new InMemoryDatabase<Microservice>(settings.databaseSettings);
            }
            factory = new DatabaseObjectFactory<Microservice>()
            {
                db = this.database
            };
        }

        [HttpGet]
        [Route("all")]
        public async Task<string> GetAllAsync()
        {
            string toReturn = "";
            foreach (Microservice ms in await factory.ReadAllAsync())
            {
                toReturn = JsonConvert.SerializeObject(await factory.ReadAllAsync());
            }
            if (String.IsNullOrEmpty(toReturn))
            {
                return "Could not find any Orgainzations.";
            }
            return toReturn;
        }

        [HttpGet]
        [Route("{dataFilter}/{data}")]
        public async Task<string> GetAsync([FromRoute]string dataFilter, [FromRoute]string data)
        {
            string toReturn = "[";
            var x = await factory.ReadFilteredAsync(dataFilter, data);
            if (x.Count != 0)
            {
                for (int i = 0; i < x.Count - 1; i++)
                {
                    toReturn = toReturn + x[i].ToJson() + ",";
                }
                toReturn = toReturn + x.Last().ToJson() + "]";

                if (!toReturn.Equals("[]"))
                {
                    return toReturn;
                }
            }
            return "Could Not Get";
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
        [Route("{id}")]
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
        [Route("{orginalId}")]
        public async Task<string> PutAsync([FromRoute] string orginalId, [FromBody] Microservice updatedService)
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
