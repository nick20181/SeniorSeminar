using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Service.Address.Implementations
{
    public class MicroService : IMicroService
    {
        public string ServiceName { get; set; }

        public INetworkSettings NetworkSettings { get; set; } = new NetworkSettings();

        public string ID { get; set; }

        public string discription { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(new MicroService()
            {
                ServiceName = ServiceName,
                NetworkSettings = NetworkSettings,
                ID = ID,
                discription = discription
            });
        }

        public async Task intilizeSettingsAsync()
        {
            await NetworkSettings.intilizeSettingsAsync();
        }
    }
}
