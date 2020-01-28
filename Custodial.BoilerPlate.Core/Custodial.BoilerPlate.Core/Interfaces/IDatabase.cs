using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Core.Interfaces
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
