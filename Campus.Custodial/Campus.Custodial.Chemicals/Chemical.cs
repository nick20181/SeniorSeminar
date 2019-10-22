using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public class Chemical : IChemical
    {
        [JsonIgnore]
        public IDatabase DB { get; set; }
        public string name;
        public bool deleted = false;

        public IChemical DeleteChemical()
        {
            return UpdateChemical(new Chemical()
            {
                DB = DB,
                name = name,
                deleted = true
            });
        }

        public IChemical UpdateChemical(IChemical updatedChemical)
        {
            string oldName = name;
            name = updatedChemical.GetName();
            deleted = updatedChemical.GetDeletedStatus();
            return DB.UpdateChemical(updatedChemical, oldName);
        }

        override
        public string ToString()
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            } else
            {
                return $"{name}";
            }
        }

        public string GetName()
        {
            return name;
        }

        public bool GetDeletedStatus()
        {
            return deleted;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(new Chemical()
            {
                name = name,
                DB = DB,
                deleted = deleted
            });
        }
    }
}
