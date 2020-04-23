using Custodial.BoilerPlate.Service_Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate
{
    public interface IDatabase
    {
        IDatabaseSettings settings { get; set; }
        Task<List<IDatabaseObject>> ReadAsync(IDatabaseObject databaseObject = null);
        Task<IDatabaseObject> UpdateAsync(IDatabaseObject databaseObjectOrginal, IDatabaseObject databaseObjectUpdated);
        Task<IDatabaseObject> CreateAsync(IDatabaseObject databaseObject);
        Task<IDatabaseObject> DeleteAsync(IDatabaseObject databaseObject);
    }

    public enum DatabaseTypes
    {
        InMemoryDatabase = 0,
        MongoDatabase = 1,
        MySqlDatabase = 2
    }
}
