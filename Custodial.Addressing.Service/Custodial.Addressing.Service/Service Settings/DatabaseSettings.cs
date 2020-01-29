using Custodial.Addressing.Service.Converters;
using Custodial.Addressing.Service.Interfaces;
using Custodial.Addressing.Service.Service_Settings.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Addressing.Service.Service_Settings
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
