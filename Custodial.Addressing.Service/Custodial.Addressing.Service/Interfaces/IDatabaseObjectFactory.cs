using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Addressing.Service.Interfaces
{
    public interface IDatabaseObjectFactory
    {
        IDatabase db { get; set; }
        Task<IDatabaseObject> CreateAsync(IDatabaseObject databaseObject);
        Task<List<IDatabaseObject>> ReadAllAsync();
        Task<List<IDatabaseObject>> ReadFilteredAsync(IDatabaseObject databaseObject);
    }
}
