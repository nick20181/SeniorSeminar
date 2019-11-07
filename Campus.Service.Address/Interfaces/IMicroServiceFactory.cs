using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Service.Address.Interfaces
{
    public interface IMicroServiceFactory
    {
        IDatabase database { get; set; }
        Task<IMicroService> CreateAsync(IMicroService microService);
        Task<List<IMicroService>> ReadAllAsync();
        Task<List<IMicroService>> ReadAsync(IMicroService microService);
    }
}
