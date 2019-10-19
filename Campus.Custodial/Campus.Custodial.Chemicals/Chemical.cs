using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public class Chemical : IChemical
    {
        public IDatabase DB { get; set; }
        public string id;
        public string name;
        public bool deleted = false;

        public void DeleteChemical()
        {
            UpdateChemical(new Chemical()
            {
                id = id,
                DB = DB,
                name = name,
                deleted = true
            });
        }

        public void UpdateChemical(IChemical updatedChemical)
        {
            id = updatedChemical.getID();
            name = updatedChemical.getName();
            deleted = updatedChemical.getDeletedStatus();
            DB.UpdateChemical(updatedChemical, id);
        }

        override
        public string ToString()
        {
            return $"{id}, {name}";
        }

        public string getID()
        {
            return id;
        }

        public string getName()
        {
            return name;
        }

        public bool getDeletedStatus()
        {
            return deleted;
        }
    }
}
