using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Service_Settings.Interfaces
{
    public interface INetworkSettings
    {
        List<string> addresses { get; set; }
        List<string> ports { get; set; }

        Task InitNetworkSettingsAsync();
    }
}
