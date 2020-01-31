using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Core.Interfaces
{
    public interface IDatabase
    {
        IDatabaseSettings settings { get; set; }
        Task<List<IDatabaseObject>> ReadAsync(string stringFilter = null, string data = null);
        Task<IDatabaseObject> UpdateAsync(string databaseObjectOrginalId, IDatabaseObject databaseObjectUpdated);
        Task<IDatabaseObject> CreateAsync(IDatabaseObject databaseObject);
        Task<IDatabaseObject> DeleteAsync(string dataObjectId);
    }

    public enum DatabaseTypes
    {
        InMemoryDatabase = 0,
        MongoDatabase = 1,
        MySqlDatabase = 2
    }
}
