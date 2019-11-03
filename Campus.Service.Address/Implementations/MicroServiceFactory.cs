using Campus.Service.Address.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Service.Address.Implementations
{
    public class MicroServiceFactory : IMicroServiceFactory
    {
        public IDatabase database { get; set; }

        public async Task<IMicroService> CreateAsync(IMicroService microService)
        {
            return await database.CreateAsync(microService);
        }

        public async Task<List<IMicroService>> ReadAllAsync(IMicroService microService)
        {
            return await database.ReadAsync(microService);
        }

        public async Task<List<IMicroService>> ReadAsync()
        {
            return await database.ReadAsync();
        }
    }
}
