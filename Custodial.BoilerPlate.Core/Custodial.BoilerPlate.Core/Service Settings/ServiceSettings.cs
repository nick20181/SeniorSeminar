using Custodial.BoilerPlate.Core.Interfaces;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Core.Service_Settings
{
    public class ServiceSettings : IServiceSettings
    {
        [JsonIgnore]
        private IResourceLoader resourceLoader = new ResourceLoader();
        [JsonConverter(typeof(ConcreteTypeConverter<NetworkSettings>))]
        public INetworkSettings networkSettings { get; set; } = new NetworkSettings();

        [JsonConverter(typeof(ConcreteTypeConverter<DatabaseSettings>))]
        public IDatabaseSettings databaseSettings { get; set; } = new DatabaseSettings();

        [JsonConverter(typeof(ConcreteTypeConverter<CustodialAddressingSettings>))]
        public ICustodialAddressingSettings casSettings { get; set; } = new CustodialAddressingSettings();

        [JsonIgnore]
        public string certSSL = $"cert";

        [JsonIgnore]
        public string passwordSSL = $"pass";

        public ServiceSettings()
        {

        }

        public Task InitServiceSettingsAsync( string resourceFile, Assembly assembly)
        {
            if (assembly == null)
            {
                assembly = this.GetType().Assembly;
            }
            string embeddedString = resourceLoader.GetEmbeddedResourceString(assembly, resourceFile);
            var temp = JsonConvert.DeserializeObject<ServiceSettings>(embeddedString);
            networkSettings = temp.networkSettings;
            databaseSettings = temp.databaseSettings;
            casSettings = temp.casSettings;
            networkSettings.InitNetworkSettingsAsync();
            return Task.CompletedTask;
        }
    }
}
