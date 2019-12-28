using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate
{
    public interface IDatabaseObject
    {
        DateTime timeCreated { get; set; }
        string iD { get; set; }
        bool isDeleted { get; set; }
        string ToJson();
        Task UpdateAsync(IDatabaseObject databaseObjectOrginal, IDatabaseObject databaseObjectUpdated);
        Task DeleteAsync(IDatabaseObject databaseObject);
    }
}
