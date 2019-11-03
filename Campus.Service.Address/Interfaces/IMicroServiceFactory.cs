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
        Task<List<IMicroService>> ReadAsync();
        Task<List<IMicroService>> ReadAllAsync(IMicroService microService);
    }
}
