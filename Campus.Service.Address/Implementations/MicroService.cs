using Campus.Service.Address.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Service.Address.Implementations
{
    public class MicroService : IMicroService
    {
        [JsonIgnore]
        public IDatabase database { get; set; }
        public string serviceName { get; set; }

        public string discription { get; set; }

        public string shortName { get; set; }
        public IServiceSettings settings { get; set; } = new ServiceSettings();

        public string ToJson()
        {
            return JsonConvert.SerializeObject(new MicroService()
            {
                serviceName = serviceName,
                settings = settings,
                discription = discription
            });
        }

        public async Task intilizeServiceAsync()
        {
            await settings.intilizeServiceAsync();
        }

        public async Task<IMicroService> UpdateAsync(IMicroService updatedMicroservice)
        {
            return await database.UpdateAsync(new MicroService()
            {
                serviceName = serviceName,
                discription = discription,
                shortName = shortName,
                settings = settings
            }, updatedMicroservice);
        }

        public async Task<IMicroService> DeleteAsync()
        {
            return await database.DeleteAsync(new MicroService()
            {
                serviceName = serviceName,
                discription = discription,
                shortName = shortName,
                settings = settings
            });
        }
    }
}
