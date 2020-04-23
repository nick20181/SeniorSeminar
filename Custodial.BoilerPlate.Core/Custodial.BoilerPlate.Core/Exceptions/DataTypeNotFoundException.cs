using System;
using System.Collections.Generic;
using System.Text;

namespace Custodial.BoilerPlate.Core.Exceptions
{
    public class DataTypeNotFoundException : Exception
    {
        public DataTypeNotFoundException()
        {
        }

        public DataTypeNotFoundException(string message) : base(message)
        {
        }
    }
}
