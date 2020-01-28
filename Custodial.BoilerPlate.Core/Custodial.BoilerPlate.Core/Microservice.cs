using Custodial.BoilerPlate.Core.Interfaces;
using Custodial.BoilerPlate.Core.Service_Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Core
{
    public class Microservice : IDatabaseObject
    {
        [BsonIgnore]
        [JsonIgnore]
        public IDatabase database { get; set; }
        public long timeCreated { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string iD { get; set; }
        public bool isDeleted { get; set; }
        public string serviceName { get; set; }
        [JsonConverter(typeof(ConcreteTypeConverter<ServiceSettings>))]
        public IServiceSettings settings { get; set; }
        public string discription { get; set; }
        public string shortName { get; set; }

        public async Task<IDatabaseObject> DeleteAsync(IDatabase database = null)
        {
            if (this.database == null)
            {
                this.database = database;
            }
            return await this.database.DeleteAsync(copyItself());
        }

        public async Task<IDatabaseObject> UpdateAsync(IDatabaseObject databaseObjectUpdated, IDatabase database = null)
        {
            if (this.database == null)
            {
                this.database = database;
            }
            return await this.database.UpdateAsync(copyItself(), databaseObjectUpdated);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(new Microservice()
            {
                timeCreated = timeCreated,
                iD = iD,
                isDeleted = isDeleted,
                serviceName = serviceName,
                settings = settings,
                discription = discription,
                shortName = shortName
            });
        }

        private IDatabaseObject copyItself()
        {
            return new Microservice()
            {
                timeCreated = timeCreated,
                iD = iD,
                isDeleted = isDeleted,
                serviceName = serviceName,
                settings = settings,
                discription = discription,
                shortName = shortName,
                database = null
            };
        }

        public IDatabaseObject NullObject()
        {
            return new Microservice()
            {
                iD = null,
                serviceName = null,
                settings = null,
                discription = null,
                shortName = null,
                database = null
            };
        }
    }
}
