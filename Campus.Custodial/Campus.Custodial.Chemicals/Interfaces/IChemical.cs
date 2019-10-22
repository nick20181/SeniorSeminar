using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public interface IChemical
    {
        public string ToString();
        public IChemical UpdateChemical(IChemical updatedChemical);
        public IChemical DeleteChemical();
        public string GetName();
        public bool GetDeletedStatus();
        public string ToJson();
    }
}
