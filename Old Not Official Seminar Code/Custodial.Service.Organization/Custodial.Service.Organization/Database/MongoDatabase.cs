
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Custodial.Service.Organization.Interfaces;
using Custodial.Service.Organization.Service_Settings;
using Custodial.Service.Organization.Service_Settings.Interfaces;
using Custodial.Service.Organization.Service_Settings.Utility;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
namespace Custodial.Service.Organization
{
    public class MongoDatabase<databaseObjectType> : IDatabase where databaseObjectType : IDatabaseObject
    {
        public IDatabaseSettings settings { get; set; }
        private MongoClient client;
        private Dictionary<string, IMongoCollection<BsonDocument>> collections = new Dictionary<string, IMongoCollection<BsonDocument>>();
        private Dictionary<string, IMongoDatabase> databaseDictionary = new Dictionary<string, IMongoDatabase>();
        private Dictionary<string, IDatabaseCollection> databaseCollections = new Dictionary<string, IDatabaseCollection>();

        public MongoDatabase(IDatabaseSettings settings)
        {
            this.settings = settings;
            client = new MongoClient($"mongodb://{settings.address}:{settings.port}");
            foreach (var dbs in settings.databaseItems)
            {
                IMongoDatabase db = client.GetDatabase(dbs.databaseName);
                if (!databaseDictionary.ContainsKey(dbs.databaseName))
                {
                    databaseDictionary.Add(dbs.databaseName, db);
                    List<string> temp = new List<string>();
                    foreach (var col in dbs.collectionNames)
                    {
                        IMongoDatabase database;
                        databaseDictionary.TryGetValue(dbs.databaseName, out database);
                        temp.Add(col);
                        collections.Add(col, database.GetCollection<BsonDocument>(col));
                    }
                    databaseCollections.Add(dbs.databaseName, new DatabaseCollection()
                    {
                        databaseName = dbs.databaseName,
                        collectionNames = temp
                    });

                }
            }
            bool MicroservceMapped = false;
            bool ServiceSettingsMapped = false;
            foreach (var map in BsonClassMap.GetRegisteredClassMaps())
            {
                if (map.ClassType.Equals(typeof(Organization)))
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
                BsonClassMap.RegisterClassMap<Organization>();
                BsonSerializer.RegisterSerializer(typeof(IDatabaseObject),
                    BsonSerializer.LookupSerializer<Organization>());
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
            }
        }

        public async Task<IDatabaseObject> CreateAsync(IDatabaseObject databaseObject)
        {
            IMongoDatabase database;
            IMongoCollection<BsonDocument> collection = null;
            foreach (var db in databaseCollections)
            {
                databaseDictionary.TryGetValue(db.Value.databaseName, out database);
                foreach (var colList in db.Value.collectionNames)
                {
                    collections.TryGetValue(colList, out collection);
                }
            }
            await collection.InsertOneAsync(convertToBson((databaseObjectType)databaseObject));
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
            Organization msr = (Organization)databaseObject;
            //create custom type here
            Organization msd = new Organization()
            {
                activeService = msr.activeService,
                organizationAddress = msr.organizationAddress,
                iD = msr.iD,
                isDeleted = true,
                organizationName = msr.organizationName,
                phoneNumber = msr.phoneNumber,
                timeCreated = msr.timeCreated
            };
            return await UpdateAsync(msr, msd);
        }

        public async Task<List<IDatabaseObject>> ReadAsync(IDatabaseObject databaseObject = null)
        {
            List<IDatabaseObject> toReturn = new List<IDatabaseObject>();
            IMongoDatabase database;
            IMongoCollection<BsonDocument> collection = null;

            foreach (var db in databaseCollections)
            {
                databaseDictionary.TryGetValue(db.Value.databaseName, out database);
                foreach (var colList in db.Value.collectionNames)
                {
                    collections.TryGetValue(colList, out collection);
                }
            }
            databaseObjectType dataObject = (databaseObjectType)databaseObject;
            FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
            {
                BatchSize = 10,
                NoCursorTimeout = false
            };
            BsonDocument filter;
            if (dataObject == null)
            {
                filter = new BsonDocument();
            }
            else
            {
                filter = convertToBson(dataObject);
            }
            IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter);
            while (await cursor.MoveNextAsync())
            {
                IEnumerable<BsonDocument> batch = cursor.Current;
                foreach (var doc in batch)
                {
                    if (dataObject == null)
                    {
                        toReturn.Add(BsonSerializer.Deserialize<databaseObjectType>(doc));
                    }
                    else
                    {
                        if (BsonSerializer.Deserialize<databaseObjectType>(doc).iD.Equals(dataObject.iD))
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
                    //await RemoveAsync(databaseObjectOrginal);
                    IMongoDatabase database;
                    IMongoCollection<BsonDocument> collection = null;

                    foreach (var db in databaseCollections)
                    {
                        databaseDictionary.TryGetValue(db.Value.databaseName, out database);
                        foreach (var colList in db.Value.collectionNames)
                        {
                            collections.TryGetValue(colList, out collection);
                        }
                    }
                    //return await CreateAsync(databaseObjectUpdated);
                    var filter = convertToBson((databaseObjectType)databaseObjectOrginal);
                    var x = await collection.ReplaceOneAsync(filter, convertToBson((databaseObjectType)databaseObjectUpdated));
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
            databaseObjectType ms = (databaseObjectType)databaseObject;
            IMongoDatabase database;
            IMongoCollection<BsonDocument> collection = null;

            foreach (var db in databaseCollections)
            {
                databaseDictionary.TryGetValue(db.Value.databaseName, out database);
                foreach (var colList in db.Value.collectionNames)
                {
                    collections.TryGetValue(colList, out collection);
                }
            }
            FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
            {
                BatchSize = 10,
                NoCursorTimeout = false
            };
            var filter = convertToBson(ms);
            foreach (var mms in await ReadAsync(ms))
            {
                if (mms.ToJson().Equals(ms.ToJson()))
                {
                    await collection.DeleteOneAsync(filter);
                }
            }
        }

        private BsonDocument convertToBson(databaseObjectType databaseObject)
        {
            Organization org = (Organization)(IDatabaseObject)databaseObject;
            return new BsonDocument
            {
                {"timeCreated", BsonDocument.Create(new BsonDocumentWrapper(org.timeCreated)) },
                {"isDeleted", BsonDocument.Create(new BsonDocumentWrapper(org.isDeleted)) },
                {"activeService", BsonDocument.Create(new BsonDocumentWrapper(org.activeService)) },
                {"organizationName", BsonDocument.Create(new BsonDocumentWrapper(org.organizationName)) },
                {"organizationAddress", BsonDocument.Create(new BsonDocumentWrapper(org.organizationAddress)) },
                {"phoneNumber", BsonDocument.Create(new BsonDocumentWrapper(org.phoneNumber)) }
            };
        }
    }
}
