using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public class Chemical : IChemical
    {
        public IDatabase DB { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public bool deleted = false;

        public IChemical DeleteChemical()
        {
            throw new NotImplementedException();
        }

        public IChemical UpdateChemical(IChemical updatedChemical)
        {
            throw new NotImplementedException();
        }
    }
}
