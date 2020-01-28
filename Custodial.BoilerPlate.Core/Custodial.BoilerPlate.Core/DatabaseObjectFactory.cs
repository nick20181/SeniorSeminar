using Custodial.BoilerPlate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Core
{
    public class DatabaseObjectFactory<databaseObject> : IDatabaseObjectFactory where databaseObject : IDatabaseObject
    {
        public IDatabase db { get; set; }

        public async Task<IDatabaseObject> CreateAsync(IDatabaseObject databaseObject)
        {
            databaseObject dataObject = (databaseObject)databaseObject;
            return await db.CreateAsync(dataObject);
        }

        public async Task<List<IDatabaseObject>> ReadAllAsync()
        {
            return await db.ReadAsync();
        }

        public async Task<List<IDatabaseObject>> ReadFilteredAsync(IDatabaseObject databaseObject)
        {
            databaseObject dataObject = (databaseObject)databaseObject;
            return await db.ReadAsync(dataObject);
        }
    }
}
