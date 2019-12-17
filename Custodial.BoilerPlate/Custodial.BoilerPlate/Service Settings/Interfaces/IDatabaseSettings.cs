using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Service_Settings.Interfaces
{
    public interface IDatabaseSettings
    {
        string address { get; set; }
        string port { get; set; }
        List<string> collectionNames { get; set; }
        List<string> databaseNames { get; set; }
    }
}
