using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Custodial.Addressing.Service.Interfaces
{
    public interface IServiceSettings
    {
        INetworkSettings networkSettings { get; }
        IDatabaseSettings databaseSettings { get; }
        Task InitServiceSettingsAsync(Assembly assembly = null, string resourceFile = "ServiceSettings.json");
    }
}
