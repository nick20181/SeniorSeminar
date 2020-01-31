using Custodial.BoilerPlate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Core.Database
{
    public class MySQLDatabase : IDatabase
    {
        public IDatabaseSettings settings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task<IDatabaseObject> CreateAsync(IDatabaseObject databaseObject)
        {
            throw new NotImplementedException();
        }

        public Task<IDatabaseObject> DeleteAsync(string dataObjectId)
        {
            throw new NotImplementedException();
        }

        public Task<List<IDatabaseObject>> ReadAsync(string stringFilter = null, string data = null)
        {
            throw new NotImplementedException();
        }

        public Task<IDatabaseObject> UpdateAsync(string databaseObjectOrginalId, IDatabaseObject databaseObjectUpdated)
        {
            throw new NotImplementedException();
        }
    }
}
