using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public interface IChemicalFactory
    {
        public IChemical ReadChemical(string id);
        public IChemical CreateChemical(string id, string name);
    }
}
