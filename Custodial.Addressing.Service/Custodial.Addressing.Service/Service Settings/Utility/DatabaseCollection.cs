using Custodial.Addressing.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Addressing.Service.Service_Settings.Utility
{
    public class DatabaseCollection : IDatabaseCollection
    {
        public string databaseName { get; set; }
        public List<string> collectionNames { get; set; }
    }
}
