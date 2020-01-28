using Custodial.BoilerPlate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Custodial.BoilerPlate.Core.Service_Settings
{
    public class CustodialAddressingSettings : ICustodialAddressingSettings
    {
        public string address { get; set; }
        public string port { get; set; }
    }
}
