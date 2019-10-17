using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public class InMemoryDatabase : IDatabase
    {
        private Dictionary<string, Chemical> DB = new Dictionary<string, Chemical>();

        /// <summary>
        /// Creates the Chemical from the Database
        /// </summary>
        /// <param name="newChemical"> Chemical to add to the Database</param>
        /// <returns>the created chemical</returns>
        public IChemical CreateChemical(Chemical newChemical)
        {
            DB.Add(newChemical.id, newChemical);
            return newChemical;
        }
        /// <summary>
        /// Deletes the Chemical from the Database
        /// Might not update the stored item in the Dictionary.
        /// </summary>
        /// <param name="targetChemical">Chemical to delete from the database</param>
        /// <returns>the deleted chemical</returns>
        public IChemical DeleteChemical(Chemical targetChemical)
        {
            Chemical target;
            if(DB.TryGetValue(targetChemical.id, out target))
            {
                target.deleted = true;
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
            Chemical target;
            if (DB.TryGetValue(id, out target))
            {
                target.deleted = true;
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
        public IChemical UpdateChemical(Chemical updatedChemical, Chemical targetChemical)
        {
            DB.Add(targetChemical.id, updatedChemical);
            return updatedChemical;
        }
    }
}
