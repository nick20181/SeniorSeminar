using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Addressing.Service.Interfaces
{
    public interface IDatabaseSettings
    {
        DatabaseTypes typeOfDatabase { get; set; }
        string address { get; set; }
        string port { get; set; }
        List<string> collectionNames { get; set; }
        List<string> databaseNames { get; set; }
    }
}
