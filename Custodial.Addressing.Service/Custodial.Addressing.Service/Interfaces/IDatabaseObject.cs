using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Addressing.Service
{
    public interface IDatabaseObject
    {
        DateTime timeCreated { get; set; }
        string iD { get; set; }
        bool isDeleted { get; set; }
        string ToJson();
        Task<IDatabaseObject> UpdateAsync(IDatabaseObject databaseObjectUpdated, IDatabase database = null);
        Task<IDatabaseObject> DeleteAsync(IDatabase database = null);
    }
}
