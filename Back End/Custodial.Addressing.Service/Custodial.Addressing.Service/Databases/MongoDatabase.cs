using Custodial.Addressing.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Custodial.Addressing.Service.Service_Settings.Utility;
using Custodial.Addressing.Service.Service_Settings;

namespace Custodial.Addressing.Service.Databases
{
    public class MongoDatabase : IDatabase
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
                if(!databaseDictionary.ContainsKey(dbs.databaseName))
                {
                    databaseDictionary.Add(dbs.databaseName, db);
                    List<string> temp = new List<string>();
                    foreach ( var col in dbs.collectionNames)
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
                if (map.ClassType.Equals(typeof(Microservice)))
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
                BsonClassMap.RegisterClassMap<Microservice>();
                BsonSerializer.RegisterSerializer(typeof(IDatabaseObject),
                    BsonSerializer.LookupSerializer<Microservice>());
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
            await collection.InsertOneAsync(convertToBson((Microservice)databaseObject));
            List<IDatabaseObject> returned = await ReadAsync();
            foreach (Microservice mms in returned)
            {
                Microservice orginal = (Microservice)databaseObject;
                var temp = mms.timeCreated.CompareTo(orginal.timeCreated);
                if (mms.isDeleted.Equals(orginal.isDeleted) && mms.shortName.Equals(orginal.shortName)
                    && mms.serviceName.Equals(orginal.serviceName) && mms.discription.Equals(orginal.discription) && mms.timeCreated.Equals(orginal.timeCreated))
                {
                    return mms;
                }
            }
            return Microservice.nullMicroService();
        }

        public async Task<IDatabaseObject> DeleteAsync(IDatabaseObject databaseObject)
        {
            Microservice msr = (Microservice)databaseObject;
            Microservice msd = new Microservice()
            {
               database = null,
               discription = msr.discription,
               iD = msr.iD,
               isDeleted = true,
               serviceName = msr.serviceName,
               settings = msr.settings,
               shortName = msr.shortName,
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
            Microservice microService = (Microservice)databaseObject;
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
                filter = new BsonDocument("serviceName", BsonValue.Create(new BsonDocumentWrapper(microService.serviceName)));
            }
            IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter);
            while (await cursor.MoveNextAsync())
            {
                IEnumerable<BsonDocument> batch = cursor.Current;
                foreach (var doc in batch)
                {
                    if (microService == null)
                    {
                        toReturn.Add(BsonSerializer.Deserialize<Microservice>(doc));
                    } else
                    {
                        if (BsonSerializer.Deserialize<Microservice>(doc).serviceName.Equals(microService.serviceName))
                        {
                            toReturn.Add(BsonSerializer.Deserialize<Microservice>(doc));
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
                    var filter = convertToBson((Microservice)databaseObjectOrginal);
                    var x = await collection.ReplaceOneAsync(filter, convertToBson((Microservice)databaseObjectUpdated));
                    if (x.IsAcknowledged && x.ModifiedCount > 0)
                    {
                        return databaseObjectUpdated;
                    } else
                    {
                        return databaseObjectOrginal;
                    }
                }
            }
            return Microservice.nullMicroService();
        }

        public async Task RemoveAsync(IDatabaseObject databaseObject)
        {
            Microservice ms = (Microservice) databaseObject; 
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

        private BsonDocument convertToBson(Microservice microService)
        {
            return new BsonDocument
            {
                {"timeCreated", BsonValue.Create(microService.timeCreated) },
                {"isDeleted", BsonValue.Create(microService.isDeleted)},
                {"serviceName", BsonValue.Create(microService.serviceName)},
                {"settings", BsonValue.Create(new BsonDocumentWrapper(microService.settings)) },
                {"discription", BsonValue.Create(microService.discription) },
                {"shortName", BsonValue.Create(microService.shortName)}
            };
        }
    }
}
