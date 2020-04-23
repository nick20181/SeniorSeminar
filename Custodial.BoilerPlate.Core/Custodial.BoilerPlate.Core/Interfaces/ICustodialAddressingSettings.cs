using System;
using System.Collections.Generic;
using System.Text;

namespace Custodial.BoilerPlate.Core.Interfaces
{
    public interface ICustodialAddressingSettings
    {
        public string address { get; set; }
        public string port { get; set; }
    }
}
