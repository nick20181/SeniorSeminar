using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public interface IDatabase
    {
        public IChemical ReadChemical(string id);
        public IChemical CreateChemical(IChemical newChemical);
        public IChemical UpdateChemical(IChemical updatedChemical, string targetChemicalID);
        public IChemical DeleteChemical(IChemical targetChemical);
    }
}
