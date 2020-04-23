using System;
using System.Collections.Generic;
using System.Text;

namespace Custodial.BoilerPlate.Core.Interfaces
{
    public interface IDatabaseSettings
    {
        DatabaseTypes typeOfDatabase { get; set; }
        string connectionString { get; set; }
        string database { get; set; }
        string collection { get; set; }
    }
}
