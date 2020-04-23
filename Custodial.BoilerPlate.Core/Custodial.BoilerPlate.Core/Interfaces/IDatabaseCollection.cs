using System;
using System.Collections.Generic;
using System.Text;

namespace Custodial.BoilerPlate.Core.Interfaces
{
    public interface IDatabaseCollection
    {
        public string databaseName { get; set; }
        public List<string> collectionNames { get; set; }
    }
}
