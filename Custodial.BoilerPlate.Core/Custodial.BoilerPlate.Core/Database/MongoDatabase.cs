using Custodial.BoilerPlate.Core.Interfaces;
using Custodial.BoilerPlate.Core.Service_Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Core.Database
{
    public partial class MongoDatabase<databaseObjectType> : IDatabase where databaseObjectType : IDatabaseObject
    {
        public IDatabaseSettings settings { get; set; }
        private IMongoDatabase db = null;
        private IMongoCollection<BsonDocument> collection = null;
        private MongoClient client;
        private Dictionary<string, IMongoCollection<BsonDocument>> collections = new Dictionary<string, IMongoCollection<BsonDocument>>();
        private Dictionary<string, IMongoDatabase> databaseDictionary = new Dictionary<string, IMongoDatabase>();
        public IBsonConverter<databaseObjectType> bsonConverter = default(IBsonConverter<databaseObjectType>);

        public MongoDatabase(IDatabaseSettings settings, IBsonConverter<databaseObjectType> bsonConverter)
        {
            this.bsonConverter = bsonConverter;
            this.settings = settings;
            client = new MongoClient(settings.connectionString);
            //might need something with collections
            db = client.GetDatabase(settings.database);
            collection = db.GetCollection<BsonDocument>(settings.collection);
            bool MicroservceMapped = false;
            bool ServiceSettingsMapped = false;
            foreach (var map in BsonClassMap.GetRegisteredClassMaps())
            {
                if (map.ClassType.Equals(typeof(databaseObjectType)))
                {
                    MicroservceMapped = true;
                }
                if (map.ClassType.Equals(typeof(ServiceSettings)))
                {
                    ServiceSettingsMapped = true;
                }
            }
            if (!MicroservceMapped)
            {
                BsonClassMap.RegisterClassMap<databaseObjectType>();
                BsonSerializer.RegisterSerializer(typeof(IDatabaseObject),
                    BsonSerializer.LookupSerializer<databaseObjectType>());
            }
            if (!ServiceSettingsMapped)
            {
                BsonClassMap.RegisterClassMap<NetworkSettings>();
                BsonSerializer.RegisterSerializer(typeof(INetworkSettings),
                  BsonSerializer.LookupSerializer<NetworkSettings>());
                BsonClassMap.RegisterClassMap<DatabaseSettings>();
                BsonSerializer.RegisterSerializer(typeof(IDatabaseSettings),
                  BsonSerializer.LookupSerializer<DatabaseSettings>());
                BsonClassMap.RegisterClassMap<ServiceSettings>();
                BsonSerializer.RegisterSerializer(typeof(IServiceSettings),
                  BsonSerializer.LookupSerializer<ServiceSettings>());
                BsonClassMap.RegisterClassMap<CustodialAddressingSettings>();
                BsonSerializer.RegisterSerializer(typeof(ICustodialAddressingSettings),
                    BsonSerializer.LookupSerializer<CustodialAddressingSettings>());
            }
        }

        public async Task<IDatabaseObject> CreateAsync(IDatabaseObject databaseObject)
        {
            await collection.InsertOneAsync(bsonConverter.convertToBson((databaseObjectType)databaseObject));
            List<IDatabaseObject> returned = await ReadAsync();
            foreach (databaseObjectType mms in returned)
            {
                databaseObjectType orginal = (databaseObjectType)databaseObject;
                var temp = mms.timeCreated.CompareTo(orginal.timeCreated);
                if (mms.isDeleted.Equals(orginal.isDeleted) && mms.timeCreated.Equals(orginal.timeCreated))
                {
                    return mms;
                }
            }
            return null;
        }

        public async Task<IDatabaseObject> DeleteAsync(IDatabaseObject databaseObject)
        {
            databaseObjectType msr = (databaseObjectType)databaseObject;
            //create custom type here

            databaseObjectType msd = JsonConvert.DeserializeObject<databaseObjectType>(JsonConvert.SerializeObject(msr));
            msd.isDeleted = true;
            return await UpdateAsync(msr, msd);
        }

        public async Task<List<IDatabaseObject>> ReadAsync(IDatabaseObject databaseObject = null)
        {
            List<IDatabaseObject> toReturn = new List<IDatabaseObject>();

            databaseObjectType microService = (databaseObjectType)databaseObject;
            FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
            {
                BatchSize = 10,
                NoCursorTimeout = false
            };
            BsonDocument filter;
            if (microService == null)
            {
                filter = new BsonDocument();
            }
            else
            {
                filter = bsonConverter.convertToBson(microService);
            }
            IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter);
            while (await cursor.MoveNextAsync())
            {
                IEnumerable<BsonDocument> batch = cursor.Current;
                foreach (var doc in batch)
                {
                    if (microService == null)
                    {
                        var temp = BsonSerializer.Deserialize<databaseObjectType>(doc);
                        if (!temp.isDeleted)
                        {
                            toReturn.Add(temp);
                        }
                    }
                    else
                    {
                        if (BsonSerializer.Deserialize<databaseObjectType>(doc).iD.Equals(microService.iD))
                        {
                            toReturn.Add(BsonSerializer.Deserialize<databaseObjectType>(doc));
                        }
                    }
                }
            }
            return toReturn;
        }

        public async Task<IDatabaseObject> UpdateAsync(IDatabaseObject databaseObjectOrginal, IDatabaseObject databaseObjectUpdated)
        {
            foreach (var service in await ReadAsync(databaseObjectOrginal))
            {
                databaseObjectUpdated.iD = service.iD;
                if (service.ToJson().Equals(databaseObjectOrginal.ToJson()))
                {
                    var filter = bsonConverter.convertToBson((databaseObjectType)databaseObjectOrginal);
                    var x = await collection.ReplaceOneAsync(filter, bsonConverter.convertToBson((databaseObjectType)databaseObjectUpdated));
                    if (x.IsAcknowledged && x.ModifiedCount > 0)
                    {
                        return databaseObjectUpdated;
                    }
                    else
                    {
                        return databaseObjectOrginal;
                    }
                }
            }
            return null;
        }

        public async Task RemoveAsync(IDatabaseObject databaseObject)
        {
            var filter = bsonConverter.convertToBson((databaseObjectType)databaseObject);
            foreach (var mms in await ReadAsync(databaseObject))
            {
                if (mms.ToJson().Equals(databaseObject.ToJson()))
                {
                    collection.FindOneAndDelete(filter);
                }
            }
        }
    }
}
