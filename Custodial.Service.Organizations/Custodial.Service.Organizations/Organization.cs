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

namespace Custodial.Service.Organizations
{
    public class Organization : IDatabaseObject
    {
        [BsonIgnore]
        [JsonIgnore]
        public IDatabase database { get; set; }
        public long timeCreated { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string iD { get; set; }
        public bool isDeleted { get; set; }
        public bool activeService { get; set; }
        public string organizationName { get; set; }
        public Address[] organizationLocations{ get; set; }
        public ContactDetails contactDetails { get; set; }
        public int employeeCount { get; set; }

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
            return new Organization()
            {
            };
        }

        public static IDatabaseObject NulledObject()
        {
            return new Organization();
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
            return new Organization()
            {
                activeService = activeService,
                organizationLocations = organizationLocations,
                iD = iD,
                isDeleted = isDeleted,
                organizationName = organizationName,
                contactDetails = contactDetails,
                timeCreated = timeCreated,
                employeeCount = employeeCount
            };
        }
    }
}
