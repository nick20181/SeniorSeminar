﻿using MySql.Data.MySqlClient;
using MySql.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public class MySQLDatabase : IDatabase
    {
        public Task<List<IChemical>> CreateChemicalAsync(IChemical newChemical)
        {
            throw new NotImplementedException();
        }

        public Task<List<IChemical>> ReadAllChemicalAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<IChemical>> ReadChemicalAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IChemical> RemoveChemicalAsync(IChemical toRemove)
        {
            throw new NotImplementedException();
        }

        public Task<List<IChemical>> UpdateChemical(IChemical updatedChemical, IChemical targetChemicalID)
        {
            throw new NotImplementedException();
        }
    }
}
