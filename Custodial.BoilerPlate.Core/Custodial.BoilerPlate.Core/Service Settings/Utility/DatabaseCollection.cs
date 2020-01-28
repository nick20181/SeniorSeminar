using Custodial.BoilerPlate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Custodial.BoilerPlate.Core.Service_Settings.Utility
{
    public class DatabaseCollection : IDatabaseCollection
    {
        public string databaseName { get; set; }
        public List<string> collectionNames { get; set; }
    }
}
