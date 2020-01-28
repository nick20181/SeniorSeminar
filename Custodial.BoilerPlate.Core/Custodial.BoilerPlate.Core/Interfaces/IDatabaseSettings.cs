using Custodial.BoilerPlate.Core.Service_Settings.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Custodial.BoilerPlate.Core.Interfaces
{
    public interface IDatabaseSettings
    {
        DatabaseTypes typeOfDatabase { get; set; }
        string address { get; set; }
        string port { get; set; }
        List<DatabaseCollection> databaseItems { get; set; }
    }
}
