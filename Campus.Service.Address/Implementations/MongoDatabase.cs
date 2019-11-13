using Campus.Service.Address.Interfaces;
using Campus.Service.Address.Interfaces.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Campus.Service.Address.Implementations
{
    public class MongoDatabase : IDatabase
    {
        public IDatabaseSettings settings { get; set; }
        private MongoClient client;
        private IMongoDatabase database;
        private Dictionary<string, IMongoCollection<BsonDocument>> collections = new Dictionary<string, IMongoCollection<BsonDocument>>();

        public MongoDatabase(IDatabaseSettings settings)
        {
            this.settings = settings;
            client = new MongoClient($"mongodb://{settings.address}:{settings.port}");
            database = client.GetDatabase(settings.databaseName);
            foreach (var collectionName in settings.collectionNames)
            {
                collections.Add(collectionName, database.GetCollection<BsonDocument>(collectionName));
            }
            bool mongomicroserviceMapped = false;
            bool microserviceMapped = false;
            bool servicesettingsMapped = false;
            foreach (var map in BsonClassMap.GetRegisteredClassMaps())
            {
                if (map.ClassType.Equals(typeof(MongoMicroService)))
                {
                    mongomicroserviceMapped = true;
                }
                if (map.ClassType.Equals(typeof(MicroService)))
                {
                    microserviceMapped = true;
                }
                if (map.ClassType.Equals(typeof(ServiceSettings)))
                {
                    servicesettingsMapped = true;
                }
            }
            if (microserviceMapped == false)
            {
                BsonClassMap.RegisterClassMap<MicroService>(); 
                BsonSerializer.RegisterSerializer(typeof(IMicroService),
                    BsonSerializer.LookupSerializer<MicroService>());
            }
            if (mongomicroserviceMapped == false)
            {
                BsonClassMap.RegisterClassMap<MongoMicroService>();
            }
            if (servicesettingsMapped == false)
            {
                BsonClassMap.RegisterClassMap<NetworkSettings>();
                BsonClassMap.RegisterClassMap<DatabaseSettings>();
                BsonClassMap.RegisterClassMap<ServiceSettings>();
            }

        }

        public async Task<IMicroService> CreateAsync(IMicroService microservice)
        {
            return (await MongoCreateAsync(microservice)).microservice;
        }

        private async Task<MongoMicroService> MongoCreateAsync(IMicroService microService)
        {
            IMongoCollection<BsonDocument> collection;
            collections.TryGetValue("Microservices", out collection);
            await collection.InsertOneAsync(convertToBson(microService));
            foreach(var mms in await MongoReadAsync(microService))
            {
                if (mms.microservice.ToJson().Equals(microService.ToJson()))
                {
                    return mms;
                }
            }
            return null;
        }

        public async Task<IMicroService> DeleteAsync(IMicroService microService)
        {
            return (await MongoDeleteAsync(microService)).microservice;
        }

        private async Task<MongoMicroService> MongoDeleteAsync(IMicroService microService)
        {
            foreach (var service in await ReadAsync(microService))
            {
                if (service.ToJson().Equals(microService.ToJson()))
                {
                    foreach (var mms in await MongoReadAsync(microService))
                    {
                        if (mms.microservice.ToJson().Equals(microService.ToJson()))
                        {
                            await RemoveAsync(microService);
                            IMongoCollection<BsonDocument> collection;
                            collections.TryGetValue("Microservices", out collection);
                            await collection.InsertOneAsync(new BsonDocument
                            {
                                {"serviceName", BsonValue.Create(mms.serviceName) },
                                {"timeCreated", BsonValue.Create(mms.timeCreated) },
                                {"microservice", BsonValue.Create(new BsonDocumentWrapper(microService))},
                                {"deletedStatus", BsonValue.Create(true) }
                            });
                            foreach (var mmsu in await MongoReadAsync(microService))
                            {
                                if (mmsu.microservice.ToJson().Equals(microService.ToJson()))
                                {
                                    return mmsu;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        public async Task RemoveAsync(IMicroService microService)
        {
            IMongoCollection<BsonDocument> collection;
            collections.TryGetValue("Microservices", out collection);
            FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
            {
                BatchSize = 10,
                NoCursorTimeout = false
            };
            var filter = new BsonDocument("microservice", BsonValue.Create(new BsonDocumentWrapper(microService)));
            MongoMicroService temp = null;
            foreach (var mms in await MongoReadAsync(microService))
            {
                if (mms.microservice.ToJson().Equals(microService.ToJson()))
                {
                    temp =  mms;
                }
            }
            await collection.DeleteOneAsync(filter);
        }

        public async Task<List<IMicroService>> ReadAsync(IMicroService microService)
        {
            List<IMicroService> toReturn = new List<IMicroService>();
            var results = await MongoReadAsync(microService);
            foreach (var mongoMicroService in results)
            {
                toReturn.Add(mongoMicroService.microservice);
            }
            return toReturn;
        }

        private async Task<List<MongoMicroService>> MongoReadAsync(IMicroService microService = null)
        {
            List<MongoMicroService> toReturn = new List<MongoMicroService>();
            IMongoCollection<BsonDocument> collection;
            collections.TryGetValue("Microservices", out collection);
            FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
            {
                BatchSize = 10,
                NoCursorTimeout = false
            };
            BsonDocument filter;
            if (microService.serviceName == null && microService.settings.networkSettings.addresses == null)
            {
                filter = new BsonDocument();
            } else
            {
                filter = new BsonDocument("serviceName", BsonValue.Create(new BsonDocumentWrapper(microService.serviceName)));
            }
            IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter);
            while (await cursor.MoveNextAsync())
            {
                IEnumerable<BsonDocument> batch = cursor.Current;
                foreach (var doc in batch)
                {


                    if (microService.serviceName == null && microService.settings.networkSettings.addresses == null)
                    {
                        toReturn.Add(BsonSerializer.Deserialize<MongoMicroService>(doc));
                    }
                    else
                    {
                        if (BsonSerializer.Deserialize<MongoMicroService>(doc).microservice.serviceName.Equals(microService.serviceName))
                        {
                            toReturn.Add(BsonSerializer.Deserialize<MongoMicroService>(doc));
                        }
                    }
                }
            }
            return await Task.FromResult(toReturn);
        }

        public async Task<IMicroService> UpdateAsync(IMicroService microService, IMicroService updatedMicroservice)
        {
            return (await MongoUpdateAsync(microService, updatedMicroservice)).microservice;
        }

        private async Task<MongoMicroService> MongoUpdateAsync(IMicroService microService, IMicroService updatedService)
        {
            foreach (var service in await ReadAsync(microService))
            {
                if (service.ToJson().Equals(microService.ToJson()))
                {
                    foreach(var mms in await MongoReadAsync(microService))
                    {
                        if (mms.microservice.ToJson().Equals(microService.ToJson()))
                        {
                            await RemoveAsync(microService);
                            IMongoCollection<BsonDocument> collection;
                            collections.TryGetValue("Microservices", out collection);
                            await collection.InsertOneAsync(new BsonDocument
                            {
                                {"serviceName", BsonValue.Create(updatedService.serviceName) },
                                {"timeCreated", BsonValue.Create(mms.timeCreated) },
                                {"microservice", BsonValue.Create(new BsonDocumentWrapper(updatedService))},
                                {"deletedStatus", BsonValue.Create(mms.deletedStatus) }
                            });
                            foreach (var mmsu in await MongoReadAsync(updatedService))
                            {
                                if (mmsu.microservice.ToJson().Equals(updatedService.ToJson()))
                                {
                                    return mmsu;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        private BsonDocument convertToBson(IMicroService microService)
        {
            MongoMicroService mms = new MongoMicroService(microService);
            return new BsonDocument
            {
                {"serviceName", BsonValue.Create(mms.serviceName) },
                {"timeCreated", BsonValue.Create(new BsonDateTime(mms.timeCreated)) },
                {"microservice", BsonValue.Create(new BsonDocumentWrapper(mms.microservice))},
                {"deletedStatus", BsonValue.Create(mms.deletedStatus) }
            };
        }


        public void workerThread(double timeout = 10000)
        {
            while (true)
            {
                List<MongoMicroService> toReturn = new List<MongoMicroService>();
                IMongoCollection<BsonDocument> collection;
                collections.TryGetValue("Microservices", out collection);
                FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
                {
                    BatchSize = 10,
                    NoCursorTimeout = false
                };
                BsonDocument filter = new BsonDocument();
                var cursor = (collection.Find(filter)).ToCursor();
                while (cursor.MoveNext())
                {
                    IEnumerable<BsonDocument> batch = cursor.Current;
                    foreach (var doc in batch)
                    {
                            toReturn.Add(BsonSerializer.Deserialize<MongoMicroService>(doc));
                    }
                }

                DateTime now = DateTime.Now;
                foreach (var service in toReturn)
                {
                    if (now.Subtract(service.timeCreated).TotalMilliseconds > timeout)
                    {
                        collection.DeleteOne(new BsonDocument
                        {
                            {"serviceName", BsonValue.Create(service.serviceName) }
                        });
                    }
                }
            }
        }

    }

    public class MongoMicroService
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime timeCreated { get; set; }
        public IMicroService microservice { get; set; } = new MicroService();
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; }
        public bool deletedStatus { get; set; } = false;
        public string serviceName { get; set; }
        public MongoMicroService(IMicroService service)
        {
            serviceName = service.serviceName;
            microservice = service;
            timeCreated = DateTime.Now;
            Console.WriteLine($"Createing MongoObject: Time: {timeCreated}");
        }
    }
}
    
