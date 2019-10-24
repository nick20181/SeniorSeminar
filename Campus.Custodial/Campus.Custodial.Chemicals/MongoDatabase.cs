﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Campus.Custodial.Chemicals
{
    public class MongoDatabase : IDatabase
    {
        public static string connectionString = $"mongodb://localhost:27017";
        private MongoClient client;
        private IMongoDatabase db;
        private IMongoCollection<BsonDocument> chemicalCollection;

        public MongoDatabase()
        {
            client = new MongoClient(connectionString);
            db = client.GetDatabase("CampusCustodialChemical");
            chemicalCollection = db.GetCollection<BsonDocument>($"Chemical");
        }

        public async Task<IChemical> CreateChemicalAsync(IChemical newChemical)
        {
            await chemicalCollection.InsertOneAsync(convertChemicalToBson(newChemical));
            return Task.FromResult(newChemical).Result;
        }

        public async Task<List<IChemical>> ReadAllChemicalAsync()
        {
            FilterDefinition<BsonDocument> filter = FilterDefinition<BsonDocument>.Empty;
            FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
            {
                BatchSize = 10,
                NoCursorTimeout = false
            };
            List<IChemical> toReturn = new List<IChemical>();
            IAsyncCursor<BsonDocument> cursor = await chemicalCollection.FindAsync(filter, options);
            while(await cursor.MoveNextAsync())
            {
                IEnumerable<BsonDocument> batch = cursor.Current;
                foreach (BsonDocument doc in batch)
                {
                    toReturn.Add(BsonSerializer.Deserialize<Chemical>(doc));
                }
            }
            return toReturn;
        }

        public async Task<List<IChemical>> ReadChemicalAsync(string id)
        {
            FilterDefinition<BsonDocument> filter = new BsonDocument("Chemical Name", id);
            FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
            {
                BatchSize = 10,
                NoCursorTimeout = false
            };
            List<IChemical> toReturn = new List<IChemical>();
            IAsyncCursor<BsonDocument> cursor = await chemicalCollection.FindAsync(filter, options);
            while (await cursor.MoveNextAsync())
            {
                IEnumerable<BsonDocument> batch = cursor.Current;
                foreach (BsonDocument doc in batch)
                {
                    toReturn.Add(BsonSerializer.Deserialize<Chemical>(doc));
                }
            }
            return toReturn;
        }

        public IChemical UpdateChemical(IChemical updatedChemical, string targetChemicalID)
        {
            throw new NotImplementedException();
        }

        public async System.Threading.Tasks.Task<bool> testMongoAsync()
        {
            var database = client.GetDatabase("CampusCustodialChemical");
            var collection = database.GetCollection<BsonDocument>($"Chemical");
            var document = new BsonDocument
            {
                {$"Name", BsonValue.Create("Blaech") }
            };
            await collection.InsertOneAsync(document);
            return true;
        }

        private BsonDocument convertChemicalToBson(IChemical chem)
        {
            BsonArray hazardStatements = new BsonArray(chem.GetHazardStatements());
            BsonDocument toReturn = new BsonDocument
            {
                {"Chemical Name", BsonValue.Create(chem.GetName())},
                {"Deleted Status", BsonValue.Create(chem.GetDeletedStatus())},
                {"Manufacturer", BsonValue.Create(new BsonDocumentWrapper(chem.GetManufacturer()))},
                {"Product Identifier", BsonValue.Create(chem.GetProductIdentifier())},
                {"Hazard Statements", BsonValue.Create(new BsonArray(chem.GetHazardStatements()))},
                {"Precaution Statements", BsonValue.Create(new BsonArray(chem.GetPrecautionStatements()))},
                {"Signal Words", BsonValue.Create(new BsonArray(chem.GetSignalWords()))}
            };
            return toReturn;
        }
    }
}