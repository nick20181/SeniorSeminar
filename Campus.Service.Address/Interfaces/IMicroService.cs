using Campus.Service.Address.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Service.Address
{
    public interface IMicroService
    {
        string serviceName { get; set; }
        IServiceSettings settings { get; set; }
        string discription { get; set; }
        string shortName { get; set; }
        Task intilizeServiceAsync();

        string ToJson();
    }
}
