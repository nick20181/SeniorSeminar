using Campus.Custodial.Chemicals.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public class Chemical : IChemical
    {
        //comment
        [JsonIgnore]
        public IDatabase DB { get; set; }
        [BsonElement("Chemical Name")]
        public string chemicalName;
        [BsonElement("Deleted Status")]
        public bool deleted = false;
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id = $"Not Set";
        [BsonElement("Manufacturer")]
        public Manufacturer manufacturer;
        [BsonElement("Product Identifier")]
        public string productIdentifier;
        [BsonElement("Signal Words")]
        public List<signalWords> sigWords = new List<signalWords>();
        [BsonElement("Hazard Statements")]
        public List<string> hazardStatements = new List<string>();
        [BsonElement("Precaution Statements")]
        public List<string> precautionStatements = new List<string>();

        public async Task<List<IChemical>> DeleteChemicalAsync()
        {
            ChemicalFactory chemFactory = new ChemicalFactory(DB);
            Chemical updatedChemical = new Chemical()
            {
                chemicalName = chemicalName,
                deleted = deleted,
                _id = _id,
                manufacturer = manufacturer,
                productIdentifier = productIdentifier,
                sigWords = sigWords,
                hazardStatements = hazardStatements,
                precautionStatements = precautionStatements
            };
            return await DB.UpdateChemical(updatedChemical, await chemFactory.ReadChemicalAsync(new Chemical()
            {
                _id = _id,
                chemicalName = chemicalName
            }));
        }

        public async Task<List<IChemical>> UpdateChemicalAsync(IChemical updatedChemical)
        {
            ChemicalFactory chemFactory = new ChemicalFactory(DB);
            return await DB.UpdateChemical(updatedChemical, await chemFactory.ReadChemicalAsync(new Chemical()
            {
                _id = _id,
                chemicalName = chemicalName
            }));
        }

        override
        public string ToString()
        {
            if (string.IsNullOrEmpty(chemicalName))
            {
                return null;
            }
            else
            {
                return $"{chemicalName}";
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(new Chemical()
            {
                chemicalName = chemicalName,
                DB = DB,
                deleted = deleted
            });
        }

        public string GetID()
        {
            return _id;
        }

        public string GetName()
        {
            return chemicalName;
        }

        public void SetID(string id)
        {
            _id = id;
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
                chemicalName = null,
                manufacturer = null,
                _id = null,
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
