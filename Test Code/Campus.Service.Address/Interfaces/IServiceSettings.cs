using Campus.Service.Address.Implementations;
using Campus.Service.Address.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Service.Address.Interfaces
{
    public interface IServiceSettings
    {
        INetworkSettings networkSettings { get; }
        IDatabaseSettings databaseSettings { get; }

        Task intilizeServiceAsync();
        //Task InitizeServiceSetting();
    }
}
