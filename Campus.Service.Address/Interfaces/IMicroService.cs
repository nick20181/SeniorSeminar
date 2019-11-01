using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Service.Address
{
    public interface IMicroService
    {
        string ServiceName { get; }
        INetworkSettings NetworkSettings { get; }
        string ID { get; }
        string discription { get; }
        Task intilizeSettingsAsync();

        string ToJson();
    }
}
