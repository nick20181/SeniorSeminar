using Campus.Service.Address.Interfaces;
using Campus.Service.Address.Interfaces.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Campus.Service.Address.Implementations
{
    public class ServiceSettings : IServiceSettings
    {
        [JsonIgnore]
        private IResourceLoader resourceLoader = new ResourceLoader();
        public INetworkSettings networkSettings { get; set; } = new NetworkSettings();

        public IDatabaseSettings databaseSettings { get; set; } = new DatabaseSettings();

        public async Task intilizeServiceAsync()
        {
            string embeddedString = resourceLoader.GetEmbeddedResourceString(this.GetType().Assembly, "ServiceSettings.json");
            var temp = JsonConvert.DeserializeObject<ServiceSettings>(embeddedString);
            networkSettings = temp.networkSettings;
            databaseSettings = temp.databaseSettings;
            await networkSettings.intilizeSettingsAsync();
            await databaseSettings.intilizeSettingsAsync();
        }
    }
}
