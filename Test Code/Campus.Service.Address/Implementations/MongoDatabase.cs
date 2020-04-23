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
            BsonClassMap.RegisterClassMap<MicroService>();
            BsonClassMap.RegisterClassMap<MongoMircoService>();
            BsonSerializer.RegisterSerializer(typeof(IMicroService), 
                BsonSerializer.LookupSerializer<MicroService>());
            
        }

        public async Task<IMicroService> CreateAsync(IMicroService microservice)
        {
            IMongoCollection<BsonDocument> collection;
            collections.TryGetValue("Microservices", out collection);
            await collection.InsertOneAsync(convertToBson(microservice));
            return await Task.FromResult(microservice);
        }

        public async Task<IMicroService> DeleteAsync(IMicroService microservice)
        {
            IMongoCollection<BsonDocument> collection;
            collections.TryGetValue("Microservices", out collection);
            FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
            {
                BatchSize = 10,
                NoCursorTimeout = false
            };
            var filter = new BsonDocument("microservice", BsonValue.Create(new BsonDocumentWrapper(microservice)));
            await collection.DeleteOneAsync(filter);
            return microservice;
        }

        public async Task<List<IMicroService>> ReadAsync(IMicroService microservice)
        {
            List<IMicroService> toReturn = new List<IMicroService>();
            IMongoCollection<BsonDocument> collection;
            collections.TryGetValue("Microservices", out collection);
            FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
            {
                BatchSize = 10,
                NoCursorTimeout = false
            };
            var filter = new BsonDocument("microservice", BsonValue.Create(new BsonDocumentWrapper(microservice)));
            IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter);
            while (await cursor.MoveNextAsync())
            {
                IEnumerable<BsonDocument> batch = cursor.Current;
                foreach (var doc in batch)
                {
                    toReturn.Add(BsonSerializer.Deserialize<MongoMircoService>(doc).microservice);
                }
            }
            return await Task.FromResult(toReturn);
        }

        public async Task<IMicroService> UpdateAsync(IMicroService microservice, IMicroService updatedMicroservice)
        {
            foreach (var service in await ReadAsync(microservice))
            {
                if (service.ToJson().Equals(microservice.ToJson()))
                {
                    await DeleteAsync(microservice);
                    await CreateAsync(updatedMicroservice);
                    return updatedMicroservice;
                }
            }
            return null;
        }

        private BsonDocument convertToBson(IMicroService microService)
        {
            MongoMircoService mms = new MongoMircoService(microService);
            return new BsonDocument
            {
                {"timeCreated", BsonValue.Create(mms.timeCreated) },
                {"microservice", BsonValue.Create(new BsonDocumentWrapper(mms.microservice))},
                {"deletedStatus", BsonValue.Create(mms.deletedStatus) }
            };
        }


    }

    public class MongoMircoService 
    {
        public DateTime timeCreated { get; set; }
        public IMicroService microservice { get; set; } = new MicroService();
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; }
        public bool deletedStatus { get; set; } = false;
        public MongoMircoService(IMicroService service)
        {
            microservice = service;
            timeCreated = System.DateTime.Now;
        }

    }
}
    
