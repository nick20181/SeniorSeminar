using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public interface IChemical
    {
        public string ToString();
        public void UpdateChemical(IChemical updatedChemical);
        public void DeleteChemical();
        public string getID();
        public string getName();
        public bool getDeletedStatus();
    }
}
