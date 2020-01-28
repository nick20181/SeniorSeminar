using Custodial.BoilerPlate.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class DataBaseObject : IDatabaseObject
    {
        [BsonIgnore]
        [JsonIgnore]
        public IDatabase database { get; set; }
        public long timeCreated { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string iD { get; set; }
        public bool isDeleted { get; set; }

        public async Task<IDatabaseObject> DeleteAsync(IDatabase database = null)
        {
            if (this.database == null)
            {
                this.database = database;
            }
            return await this.database.DeleteAsync(new DataBaseObject()
            {
                timeCreated = timeCreated,
                iD = iD,
                isDeleted = isDeleted
            });
        }

        public IDatabaseObject NullObject()
        {
            return new DataBaseObject();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(new DataBaseObject()
            {
                timeCreated = timeCreated,
                iD = iD,
                isDeleted = isDeleted
            });
        }

        public async Task<IDatabaseObject> UpdateAsync(IDatabaseObject databaseObjectUpdated, IDatabase database = null)
        {
            if (this.database == null)
            {
                this.database = database;
            }
            return await this.database.UpdateAsync(new DataBaseObject()
            {
                timeCreated = timeCreated,
                iD = iD,
                isDeleted = isDeleted
            }, databaseObjectUpdated);
        }
    }
}
