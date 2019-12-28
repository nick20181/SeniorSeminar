using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate
{
    public interface IDatabaseObjectFactory
    {
        IDatabase db { get; set; }
        Task CreateAsync(IDatabaseObject databaseObject);
        Task ReadAllAsync();
        Task ReadFilteredAsync(IDatabaseObject databaseObject);
    }
}
