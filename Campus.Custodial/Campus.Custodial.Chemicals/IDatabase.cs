using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public interface IDatabase
    {
        public IChemical ReadChemical(string id);
        public IChemical CreateChemical(Chemical newChemical);
        public IChemical UpdateChemical(Chemical updatedChemical, Chemical targetChemical);
        public IChemical DeleteChemical(Chemical targetChemical);
    }
}
