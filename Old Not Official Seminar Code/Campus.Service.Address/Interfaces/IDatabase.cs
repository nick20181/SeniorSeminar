using Campus.Service.Address.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Service.Address.Interfaces
{
    public interface IDatabase
    {
        IDatabaseSettings settings { get; }

        Task<List<IMicroService>> ReadAsync(IMicroService microservice);
        Task<IMicroService> UpdateAsync(IMicroService microservice, IMicroService updatedMicroservice);
        Task<IMicroService> CreateAsync(IMicroService microservice);
        Task<IMicroService> DeleteAsync(IMicroService microservice);
    }
}
