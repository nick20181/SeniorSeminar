using Custodial.BoilerPlate.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Custodial.BoilerPlate.Core.Service_Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string connectionString { get; set; }
        public DatabaseTypes typeOfDatabase { get; set; } = DatabaseTypes.InMemoryDatabase;
        public string database { get; set; }
        public string collection { get; set; }
    }
}
