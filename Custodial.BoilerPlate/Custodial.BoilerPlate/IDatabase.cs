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
        Task ReadAsync(IDatabaseObject databaseObject);
        Task UpdateAsync(IDatabaseObject databaseObjectOrginal, IDatabaseObject databaseObjectUpdated);
        Task CreateAsync(IDatabaseObject databaseObject);
        Task DeleteAsync(IDatabaseObject databaseObject);
    }
}
