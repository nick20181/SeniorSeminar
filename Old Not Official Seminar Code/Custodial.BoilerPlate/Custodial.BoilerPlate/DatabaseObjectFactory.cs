using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate
{
    public class DatabaseObjectFactory<databaseObjectType> : IDatabaseObjectFactory where databaseObjectType : IDatabaseObject
    {
        public IDatabase db { get; set; }

        public async Task<IDatabaseObject> CreateAsync(IDatabaseObject databaseObject)
        {
            databaseObjectType dataObject = (databaseObjectType)databaseObject;
            return await db.CreateAsync(dataObject);
        }

        public async Task<List<IDatabaseObject>> ReadAllAsync()
        {
            return await db.ReadAsync();
        }

        public async Task<List<IDatabaseObject>> ReadFilteredAsync(IDatabaseObject databaseObject)
        {
            databaseObjectType dataObject = (databaseObjectType)databaseObject;
            return await db.ReadAsync(dataObject);
        }
    }
}
