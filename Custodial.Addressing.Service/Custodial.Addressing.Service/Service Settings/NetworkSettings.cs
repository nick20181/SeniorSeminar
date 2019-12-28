using Custodial.Addressing.Service.Service_Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Custodial.Addressing.Service.Service_Settings
{
    public class NetworkSettings : INetworkSettings
    {
        public List<string> addresses { get; set; }
        public List<string> ports { get; set; }

        public NetworkSettings()
        {

        }

        public async Task InitNetworkSettingsAsync()
        {
            addresses.Add($"localhost");
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var address in host.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    addresses.Add(address.ToString());
                }
            }
        }
    }
}
