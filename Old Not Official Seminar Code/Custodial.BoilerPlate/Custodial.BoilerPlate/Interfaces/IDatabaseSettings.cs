using Custodial.BoilerPlate.Service_Settings.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Service_Settings.Interfaces
{
    public interface IDatabaseSettings
    {
        DatabaseTypes typeOfDatabase { get; set; }
        string address { get; set; }
        string port { get; set; }
        List<DatabaseCollection> databaseItems { get; set; }
    }
}
