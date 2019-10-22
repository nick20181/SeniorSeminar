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
            IChemical temp;
            if (DB.TryGetValue(newChemical.GetName(), out temp))
            {
                return null;
            }
            DB.Add(newChemical.GetName(), newChemical);
            return newChemical;
        }

        public List<IChemical> ReadAllChemical()
        {
            List<IChemical> toreturn = new List<IChemical>();
            foreach(var x in DB.Keys)
            {
                IChemical toAdd;
                DB.TryGetValue(x, out toAdd);
                toreturn.Add(toAdd);
            }
            return toreturn;
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
                return new Chemical()
                {
                    name = null,
                    deleted = true
                };
            }
        }

        /// <summary>
        /// Updates the chemical
        /// </summary>
        /// <param name="updatedChemical">The updated Chemical</param>
        /// <param name="targetChemical">The target Chemical</param>
        /// <returns>the updated chemical</returns>
        public IChemical UpdateChemical(IChemical updatedChemical, string targetChemical)
        {
            IChemical removed;
            if (DB.TryGetValue(targetChemical, out removed))
            {
                DB.Remove(targetChemical);
            }
            DB.Add(updatedChemical.GetName(), updatedChemical);
            return updatedChemical;
        }
    }
}
