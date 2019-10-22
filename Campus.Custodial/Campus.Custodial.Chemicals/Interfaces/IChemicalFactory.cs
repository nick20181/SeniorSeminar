using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public interface IChemicalFactory
    {
        public IChemical ReadChemical(string name);
        public List<IChemical> ReadAllChemicals();
        public IChemical CreateChemical(string name);
        public IDatabase getDB();
    }
}
