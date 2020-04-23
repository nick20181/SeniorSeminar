using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Core.Interfaces
{
    public interface IDatabaseObjectFactory
    {
        IDatabase db { get; set; }
        Task<IDatabaseObject> CreateAsync(IDatabaseObject databaseObject);
        Task<List<IDatabaseObject>> ReadAllAsync();
        Task<List<IDatabaseObject>> ReadFilteredAsync(string dataFilter, string data);
    }
}
