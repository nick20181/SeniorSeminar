﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public class InMemoryDatabase : IDatabase
    {
        private Dictionary<string, IChemical> DB = new Dictionary<string, IChemical>();

        /// <summary>
        /// Creates the Chemical from the Database
        /// </summary>
        /// <param name="newChemical"> Chemical to add to the Database</param>
        /// <returns>the created chemical</returns>
        public async Task<IChemical> CreateChemicalAsync(IChemical newChemical)
        {
            IChemical temp;
            if (DB.TryGetValue(newChemical.GetName(), out temp))
            {
                return null;
            }
            DB.Add(newChemical.GetName(), newChemical);
            return  Task.FromResult(newChemical).Result;
        }

        public async Task<List<IChemical>> ReadAllChemicalAsync()
        {
            List<IChemical> toreturn = new List<IChemical>();
            foreach(var x in DB.Keys)
            {
                IChemical toAdd;
                DB.TryGetValue(x, out toAdd);
                toreturn.Add(toAdd);
            }
            return Task.FromResult(toreturn).Result;
        }

        /// <summary>
        /// Gets the chemical based on the id
        /// </summary>
        /// <param name="id">id of the chemical requested</param>
        /// <returns>the requested chemical</returns>
        public async Task<List<IChemical>> ReadChemicalAsync(string id)
        {
            List<IChemical> toReturn = new List<IChemical>();
            IChemical target;
            if (DB.TryGetValue(id, out target))
            {
                toReturn.Add(target);
                return toReturn;
            }
            else
            {
                toReturn.Add(new Chemical().NullChemical());
                return Task.FromResult(toReturn).Result;
            }
        }

        public Task<IChemical> RemoveChemicalAsync(IChemical toRemove)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the chemical
        /// </summary>
        /// <param name="updatedChemical">The updated Chemical</param>
        /// <param name="targetChemical">The target Chemical</param>
        /// <returns>the updated chemical</returns>
        public Task<List<IChemical>> UpdateChemical(IChemical updatedChemical, IChemical targetChemical)
        {
            throw new NotImplementedException();
        //    IChemical removed;
        //    if (DB.TryGetValue(targetChemical, out removed))
        //    {
        //        DB.Remove(targetChemical);
        //    }
        //    DB.Add(updatedChemical.GetName(), updatedChemical);
        //    return updatedChemical;
        }

        Task<List<IChemical>> IDatabase.CreateChemicalAsync(IChemical newChemical)
        {
            throw new NotImplementedException();
        }
    }
}
