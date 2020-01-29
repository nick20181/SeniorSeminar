using Custodial.Service.Organization;
using Custodial.Service.Organization.Interfaces;
using Custodial.Service.Organization.Service_Settings;
using Custodial.Service.Organization.Database;
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
