using Custodial.BoilerPlate.Core;
using Custodial.BoilerPlate.Core.Interfaces;
using Custodial.Services.Utility;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Service.Chemical
{
    public class Chemical : IDatabaseObject
    {
        [BsonIgnore]
        [JsonIgnore]
        public IDatabase database { get; set; }
        public long timeCreated { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string iD { get; set; }
        public bool isDeleted { get; set; }
        public string organizationId { get; set; }
        public string chemicalName { get; set; }
        public Manufactor chemicalManufactor { get; set; }
        public ContactDetails saftyContactInformation { get; set; }
        public string chemicalWarning { get; set; }
        public bool disinfectant { get; set; }
        public bool ventilationNeeded { get; set; }
        public ChemicalDetails usesAndPrep { get; set; }


        public async Task<IDatabaseObject> DeleteAsync(IDatabase database = null)
        {
            if (this.database == null)
            {
                this.database = database;
            }
            return await this.database.DeleteAsync(iD);
        }

        public IDatabaseObject NullObject()
        {
            return new Chemical()
            {
            };
        }

        public static IDatabaseObject NulledObject()
        {
            return new Chemical();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(copyIteself());
        }

        public async Task<IDatabaseObject> UpdateAsync(IDatabaseObject databaseObjectUpdated, IDatabase database = null)
        {
            if (this.database == null)
            {
                this.database = database;
            }
            return await this.database.UpdateAsync(iD, databaseObjectUpdated);

        }

        private IDatabaseObject copyIteself()
        {
            return new Chemical()
            {
                iD = iD,
                isDeleted = isDeleted,
                timeCreated = timeCreated,
                organizationId = organizationId,
                chemicalName = chemicalName,
                chemicalManufactor = chemicalManufactor,
                saftyContactInformation = saftyContactInformation,
                chemicalWarning = chemicalWarning,
                disinfectant = disinfectant,
                ventilationNeeded = ventilationNeeded,
                usesAndPrep = usesAndPrep
            };
        }
    }
}
