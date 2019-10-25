using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public interface IDatabase
    {
        public Task<List<IChemical>> ReadChemicalAsync(string id);
        public Task<List<IChemical>> ReadAllChemicalAsync();
        public Task<List<IChemical>> UpdateChemical(IChemical updatedChemical, IChemical targetChemicalID);
        public Task<List<IChemical>> CreateChemicalAsync(IChemical newChemical);
        public Task<IChemical> RemoveChemicalAsync(IChemical toRemove);
    }
}
