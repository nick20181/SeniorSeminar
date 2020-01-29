using Custodial.BoilerPlate.Service_Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate
{
    public class MySQLDatabase : IDatabase
    {
        public IDatabaseSettings settings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
