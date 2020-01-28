using Custodial.BoilerPlate.Core.Interfaces;
using Custodial.BoilerPlate.Core.Service_Settings.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Custodial.BoilerPlate.Core.Service_Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string address { get; set; }
        public string port { get; set; }

        [JsonConverter(typeof(ConcreteTypeConverter<List<DatabaseCollection>>))]
        public List<DatabaseCollection> databaseItems { get; set; }
        public DatabaseTypes typeOfDatabase { get; set; } = DatabaseTypes.InMemoryDatabase;
    }
}
