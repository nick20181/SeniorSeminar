using Campus.Service.Address.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Service.Address.Implementations
{
    public class MicroService : IMicroService
    {
        [BsonIgnore]
        [JsonIgnore]
        public IDatabase database { get; set; }
        public string serviceName { get; set; }

        public string discription { get; set; }

        public string shortName { get; set; }
        public IServiceSettings settings { get; set; } = new ServiceSettings();

        public MicroService (string serviceName, string discription, string shortName, IServiceSettings settings)
        {
            this.serviceName = serviceName;
            this.settings = settings;
            this.shortName = shortName;
            this.discription = discription;
        }

        public MicroService()
        {
        }

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
