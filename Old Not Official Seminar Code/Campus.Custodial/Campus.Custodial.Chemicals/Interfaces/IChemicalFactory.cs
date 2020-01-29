using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Campus.Custodial.Chemicals.Chemical;

namespace Campus.Custodial.Chemicals
{
    public interface IChemicalFactory
    {
        public Task<IChemical> ReadChemicalAsync(IChemical chem);
        public Task<List<IChemical>> ReadAllChemicalsAsync();
        public Task<IChemical> CreateChemicalAsync(IChemical chem);
        public IDatabase getDB();
    }
}