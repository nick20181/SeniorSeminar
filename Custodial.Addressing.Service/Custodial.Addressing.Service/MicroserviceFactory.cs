using Custodial.Addressing.Service.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Addressing.Service
{
    public class MicroserviceFactory : IDatabaseObjectFactory
    {
        public IDatabase db { get; set; }

        public async Task<IDatabaseObject> CreateAsync(IDatabaseObject databaseObject)
        {
            Microservice microsevice = (Microservice)databaseObject;
            return await db.CreateAsync(microsevice);
        }

        public async Task<List<IDatabaseObject>> ReadAllAsync()
        {
            return await db.ReadAsync();
        }

        public async Task<List<IDatabaseObject>> ReadFilteredAsync(IDatabaseObject databaseObject)
        {
            Microservice microservice = (Microservice)databaseObject;
            return await db.ReadAsync(microservice);
        }
    }
}
