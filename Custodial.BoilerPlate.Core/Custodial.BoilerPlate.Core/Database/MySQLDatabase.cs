﻿using Custodial.BoilerPlate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Core.Database
{
    public class MySQLDatabase : IDatabase
    {
        public IDatabaseSettings settings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task<IDatabaseObject> CreateAsync(IDatabaseObject databaseObject)
        {
            throw new NotImplementedException();
        }

        public Task<IDatabaseObject> DeleteAsync(IDatabaseObject databaseObject)
        {
            throw new NotImplementedException();
        }

        public Task<List<IDatabaseObject>> ReadAsync(IDatabaseObject databaseObject = null)
        {
            throw new NotImplementedException();
        }

        public Task<IDatabaseObject> UpdateAsync(IDatabaseObject databaseObjectOrginal, IDatabaseObject databaseObjectUpdated)
        {
            throw new NotImplementedException();
        }
    }
}