using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Service.Address.Implementations
{
    public class NetworkSettings : INetworkSettings
    {
        [JsonIgnore]
        private IResourceLoader resourceLoader = new ResourceLoader();
        [JsonIgnore]
        private JsonSerializer json = new JsonSerializer();
        [JsonIgnore]
        public List<IPAddress> ipAdress { get; set; }
        public string port { get; set; }
        public string databaseAddress { get; set; }
        public string databasePort { get; set; }

        public async Task intilizeSettingsAsync()
        {
            ipAdress = new List<IPAddress>();
            foreach (var address in await Dns.GetHostAddressesAsync(Dns.GetHostName()))
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAdress.Add(address);
                }
            }
            string embeddedString = resourceLoader.GetEmbeddedResourceString(this.GetType().Assembly, "NetworkSettings.json");
            var temp = JsonConvert.DeserializeObject<NetworkSettings>(embeddedString);
            ipAdress = temp.ipAdress;
            port = temp.port;
            databaseAddress = temp.databaseAddress;
            databasePort = temp.databasePort;
            
        }

    }
}
