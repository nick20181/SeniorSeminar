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
        public List<string> addresses { get; set; }
        public string port { get; set; }

        public async Task intilizeSettingsAsync()
        {
            addresses.Add($"localhost");
            foreach (var address in await Dns.GetHostAddressesAsync(Dns.GetHostName()))
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    addresses.Add(address.ToString());
                }
            }
        }

    }
}
