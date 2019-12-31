using Custodial.Addressing.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Addressing.Service.Databases
{
    public class MongoDatabase : IDatabase
    {
        public IDatabaseSettings settings { get; set; }

        public Task<IDatabaseObject> CreateAsync(IDatabaseObject databaseObject)
        {
            throw new NotImplementedException();
        }

        public Task<IDatabaseObject> DeleteAsync(IDatabaseObject databaseObject)
        {
            throw new NotImplementedException();
        }

        public Task<List<IDatabaseObject>> ReadAsync(IDatabaseObject databaseObject = null)
        {
            throw new NotImplementedException();
        }

        public Task<IDatabaseObject> UpdateAsync(IDatabaseObject databaseObjectOrginal, IDatabaseObject databaseObjectUpdated)
        {
            throw new NotImplementedException();
        }
    }
}
