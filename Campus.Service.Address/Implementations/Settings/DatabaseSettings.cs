using Campus.Service.Address.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Campus.Service.Address.Implementations
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string address { get; set; }
        public string port { get; set; }
        public List<string> collectionNames { get; set; }
        public string databaseName { get; set; }

        public Task intilizeSettingsAsync()
        {
            return Task.CompletedTask;
        }
    }
}
