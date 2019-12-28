using Custodial.BoilerPlate.Service_Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Service_Settings
{
    public class NetworkSettings : INetworkSettings
    {
        public List<string> addresses { get; set; }
        public List<string> ports { get; set; }

        public async Task InitNetworkSettingsAsync()
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
