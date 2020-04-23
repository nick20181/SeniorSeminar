using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Core.Interfaces
{
    public interface IServiceSettings
    {
        INetworkSettings networkSettings { get; }
        IDatabaseSettings databaseSettings { get; }
        ICustodialAddressingSettings casSettings { get; }
        Task InitServiceSettingsAsync(string resourceFile, Assembly assembly);
    }
}
