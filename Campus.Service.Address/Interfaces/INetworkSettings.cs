using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Campus.Service.Address
{
    public interface INetworkSettings
    {
        List<IPAddress> ipAdress { get; set; }
        string port { get; set; }
        string databaseAddress { get; set; }
        string databasePort { get; set; }
        Task intilizeSettingsAsync();
    }
}
