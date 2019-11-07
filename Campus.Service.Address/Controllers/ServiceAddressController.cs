using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Campus.Service.Address.Implementations;
using Campus.Service.Address.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Campus.Service.Address.Controllers
{
    [ApiController]
    public class ServiceAddressController : Controller
    {
        IServiceSettings settings = new ServiceSettings();
        IDatabase database;
        IMicroServiceFactory microServiceFactory;

        public ServiceAddressController()
        {
            database = new MongoDatabase(settings.databaseSettings);
            microServiceFactory = new MicroServiceFactory()
            {
                database = database
            };

        }

        [HttpGet("service/{serviceName}")]
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

        [HttpGet("serivce/all")]
        public async Task<string> GetAsync()
        {
            string toReturn = $"";
            foreach (var service in await microServiceFactory.ReadAsync(new MicroService()))
            {
                toReturn = toReturn + service.ToJson();
            }
            return toReturn;
        }

        [HttpPost("service/create")]
        public async Task<string> PostAsync(IMicroService service)
        {
            return (await microServiceFactory.CreateAsync(service)).ToJson();
        }

        [HttpPut("service/{serviceName}")]
        public string Put(string serviceName)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("service/{serviceName}")]
        public string Delete()
        {
            throw new NotImplementedException();
        }

    }
}