using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Campus.Service.Address
{
    public interface INetworkSettings
    {
        List<string> addresses { get; set; }
        string port { get; set; }

        Task intilizeSettingsAsync();
    }
}
