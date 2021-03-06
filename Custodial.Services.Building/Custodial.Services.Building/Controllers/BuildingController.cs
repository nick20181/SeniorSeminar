﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Custodial.BoilerPlate.Core;
using Custodial.BoilerPlate.Core.Database;
using Custodial.BoilerPlate.Core.Interfaces;
using Custodial.BoilerPlate.Core.Service_Settings;
using Custodial.Services.Building.Convertors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Custodial.Services.Building.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowOrigin")]
    public class BuildingController : Controller
    {
        public IDatabase database { get; set; }
        public IDatabaseObjectFactory factory { get; set; }
        public IServiceSettings settings { get; set; }

        public BuildingController()
        {
            if ((settings == null) || (settings.databaseSettings == null) || (settings.networkSettings == null))
            {
                settings = new ServiceSettings();
                settings.InitServiceSettingsAsync("ServiceSettings.json", this.GetType().Assembly);
            }
            if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
            {
                MongoDatabase<Building> mongoDB = new MongoDatabase<Building>(settings.databaseSettings, new BsonBuildingConverter());
                database = mongoDB;
            }
            else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MySqlDatabase))
            {
                database = new MySQLDatabase();
            }
            else
            {
                database = new InMemoryDatabase<Building>(settings.databaseSettings);
            }
            factory = new DatabaseObjectFactory<Building>()
            {
                db = this.database
            };
        }

        [HttpGet]
        [Route("{dataFilter}/{data}")]
        public async Task<string> GetAsync(string dataFilter, string data)
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

        [HttpGet]
        [Route("all")]
        public async Task<string> GetAllAsync()
        {
            string toReturn = "";
            foreach (Building ms in await factory.ReadAllAsync())
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
        public async Task<string> PostAsync([FromBody] Building service)
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
        [Route("{id}")]
        public async Task<string> PutAsync([FromRoute] string id, [FromBody] Building updatedService)
        {
            foreach (var dataObject in await factory.ReadFilteredAsync("_id", id))
            {
                if (dataObject.iD.Equals(id))
                {
                    return (await dataObject.UpdateAsync(updatedService, database)).ToJson();
                }
            }
            return $"Could Not update data object with id: {id}";
        }
    }
}