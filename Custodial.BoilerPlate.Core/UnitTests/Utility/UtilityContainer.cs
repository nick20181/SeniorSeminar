using Custodial.BoilerPlate;
using Custodial.BoilerPlate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Utility
{
    public class UtilityContainer
    {
        public bool isUsed = false;
        public IDatabaseObject service;
        public UtilityContainer(IDatabaseObject ob)
        {
            service = ob;
        }
    }
}
