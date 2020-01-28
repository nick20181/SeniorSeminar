using Custodial.BoilerPlate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Core.Service_Settings
{
    public class NetworkSettings : INetworkSettings
    {
        public List<string> addresses { get; set; }
        public List<string> ports { get; set; }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task InitNetworkSettingsAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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
