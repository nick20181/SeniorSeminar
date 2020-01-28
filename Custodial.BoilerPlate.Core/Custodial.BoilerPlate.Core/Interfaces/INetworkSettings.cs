using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Core.Interfaces
{
    public interface INetworkSettings
    {
        List<string> addresses { get; set; }
        List<string> ports { get; set; }

        Task InitNetworkSettingsAsync();
    }
}
