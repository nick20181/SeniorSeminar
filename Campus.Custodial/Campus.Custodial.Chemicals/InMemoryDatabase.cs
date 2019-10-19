using System;
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
        public IChemical CreateChemical(IChemical newChemical)
        {
            DB.Add(newChemical.getID(), newChemical);
            return newChemical;
        }
        /// <summary>
        /// Deletes the Chemical from the Database
        /// Might not update the stored item in the Dictionary.
        /// </summary>
        /// <param name="targetChemical">Chemical to delete from the database</param>
        /// <returns>the deleted chemical</returns>
        public IChemical DeleteChemical(IChemical targetChemical)
        {
            IChemical target;
            if(DB.TryGetValue(targetChemical.getID(), out target))
            {
                DB.Remove(target.getID());
                return target;
            } else
            {
                //Throw not found exception
                return null;
            }
        }

        /// <summary>
        /// Gets the chemical based on the id
        /// </summary>
        /// <param name="id">id of the chemical requested</param>
        /// <returns>the requested chemical</returns>
        public IChemical ReadChemical(string id)
        {
            IChemical target;
            if (DB.TryGetValue(id, out target))
            {
                return target;
            }
            else
            {
                //Throw not found exception
                return null;
            }
        }

        /// <summary>
        /// Updates the chemical
        /// </summary>
        /// <param name="updatedChemical">The updated Chemical</param>
        /// <param name="targetChemical">The target Chemical</param>
        /// <returns>the updated chemical</returns>
        public IChemical UpdateChemical(IChemical updatedChemical, string targetChemicalId)
        {
            IChemical removed;
            if (DB.TryGetValue(targetChemicalId, out removed))
            {
                DB.Remove(updatedChemical.getID());
            }
            DB.Add(targetChemicalId, updatedChemical);
            return updatedChemical;
        }
    }
}
