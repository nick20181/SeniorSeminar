using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Campus.Service.Address.Interfaces.Settings
{
    public interface IDatabaseSettings
    {
        string address { get; set; }
        string port { get; set; }
        List<string> collectionNames { get; set; }
        string databaseName { get; set; }

        Task intilizeSettingsAsync();

    }
}
