﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Service.Organization.Interfaces
{
    public interface IDatabaseCollection
    {
        public string databaseName { get; set; }
        public List<string> collectionNames { get; set; }
    }
}
