using Custodial.Addressing.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AddressingUnitTests.Utility
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
