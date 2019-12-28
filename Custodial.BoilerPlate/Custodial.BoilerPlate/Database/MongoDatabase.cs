using Custodial.BoilerPlate.Service_Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate
{
    public class MongoDatabase : IDatabase
    {
        public IDatabaseSettings settings { get; set; }

        public Task CreateAsync(IDatabaseObject databaseObject)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IDatabaseObject databaseObject)
        {
            throw new NotImplementedException();
        }

        public Task ReadAsync(IDatabaseObject databaseObject)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IDatabaseObject databaseObjectOrginal, IDatabaseObject databaseObjectUpdated)
        {
            throw new NotImplementedException();
        }
    }
}
