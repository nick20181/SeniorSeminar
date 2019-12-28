using Custodial.BoilerPlate.Service_Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Service_Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string address { get; set; }
        public string port { get; set; }
        public List<string> collectionNames { get; set; }
        public List<string> databaseNames { get; set; }
    }
}
