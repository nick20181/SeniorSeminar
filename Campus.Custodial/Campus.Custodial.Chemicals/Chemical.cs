using Campus.Custodial.Chemicals.Interfaces;
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
        public string id = $"Not Set";
        public Manufacturer manufacturer;
        public string productIdentifier;
        public List<signalWords> sigWords = new List<signalWords>();
        public List<string> hazardStatements = new List<string>();
        public List<string> precautionStatements = new List<string>();

        public IChemical DeleteChemical()
        {
            return UpdateChemical(new Chemical()
            {
                DB = DB,
                name = name,
                id = id,
                deleted = true,
                manufacturer = manufacturer,
                productIdentifier = productIdentifier,
                sigWords = sigWords,
                hazardStatements = hazardStatements,
                precautionStatements = precautionStatements
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
            }
            else
            {
                return $"{name}";
            }
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

        public string GetID()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            return name;
        }

        public bool GetDeletedStatus()
        {
            return deleted;
        }

        public Manufacturer GetManufacturer()
        {
            return manufacturer;
        }

        public string GetProductIdentifier()
        {
            return productIdentifier;
        }

        public List<string> GetHazardStatements()
        {
            return hazardStatements;
        }

        public List<string> GetPrecautionStatements()
        {
            return precautionStatements;
        }
        public List<signalWords> GetSignalWords()
        {
            return sigWords;
        }

        public enum signalWords
        {
            Danger = 0,
            Warning = 1
        }

        public IChemical NullChemical()
        {
            return new Chemical()
            {
                name = null,
                manufacturer = null,
                id = null,
                deleted = true,
                DB = null,
                productIdentifier = null,
                hazardStatements = null,
                sigWords = null,
                precautionStatements = null
            };
        }

    }  
}
