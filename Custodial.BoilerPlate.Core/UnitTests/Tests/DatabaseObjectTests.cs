using Custodial.BoilerPlate.Core.Database;
using Custodial.BoilerPlate.Core.Interfaces;
using Custodial.BoilerPlate.Core.Service_Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Utility;

namespace UnitTests
{
    [TestClass]
    public class DatabaseObjectTests
    {
        private IServiceSettings settings = new ServiceSettings();
        private IResourceLoader resourceLoader = new ResourceLoader();
        private UtilityContainerList testItems = new UtilityContainerList(10);
        private IDatabase database;

        public DatabaseObjectTests()
        {
            settings.InitServiceSettingsAsync("ServiceSettings.json", this.GetType().Assembly);
            if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.InMemoryDatabase))
            {
                database = new InMemoryDatabase<IDatabaseObject>(settings.databaseSettings)
                {
                    settings = settings.databaseSettings
                };
            }
            else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
            {
                database = new MongoDatabase<DataBaseObject>(settings.databaseSettings, new BsonDatabaseObjectConverter());
            }
            else
            {
                database = new MySQLDatabase();
            }
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs\\MicroserviceTests")
                .CreateLogger();
            Log.Logger.Information("\n");

        }

        [TestMethod]
        public async Task UpdateTestAsync()
        {
            List<IDatabaseObject> listToRemove = new List<IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertItems = new Dictionary<IDatabaseObject, IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertOutput = new Dictionary<IDatabaseObject, IDatabaseObject>();
            //Adding items and updating them
            List<IDatabaseObject> listOfExpected = new List<IDatabaseObject>();
            for (int i = 0; i < 3; i++)
            {
                IDatabaseObject ms = testItems.GetItem();
                IDatabaseObject msCreated = await database.CreateAsync(ms);
                ms.iD = msCreated.iD;

                IDatabaseObject msUpdated = testItems.GetItem();
                msUpdated.iD = ms.iD;
                listToRemove.Add(msUpdated);
                ms.database = database;
                assertOutput.Add(msUpdated, await ms.UpdateAsync(msUpdated));
                listOfExpected.Add(msUpdated);
            }
            //Geting assert items from readAsync
            foreach (IDatabaseObject actual in await database.ReadAsync())
            {
                foreach (IDatabaseObject expected in listOfExpected.ToArray())
                {
                    if (actual.ToJson() == expected.ToJson())
                    {
                        assertItems.Add(expected, actual);

                    }
                }
            }
            //Cleaning up database
            foreach (var ms in listToRemove)
            {
                if (database.settings.typeOfDatabase.Equals(DatabaseTypes.InMemoryDatabase))
                {
                }
                else if (database.settings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
                {
                    MongoDatabase<IDatabaseObject> db = (MongoDatabase<IDatabaseObject>)database;
                    await db.RemoveAsync(ms);
                }
                else if (database.settings.typeOfDatabase.Equals(DatabaseTypes.MySqlDatabase))
                {

                }
            }
            foreach (var expected in listOfExpected)
            {
                IDatabaseObject actual;
                //Testing the item read from the database useing readAsync
                assertItems.TryGetValue(expected, out actual);
                Assert.AreEqual(expected.ToJson(), actual.ToJson());
                //Testing the item returned from update method
                assertOutput.TryGetValue(expected, out actual);
                Assert.AreEqual(expected.ToJson(), actual.ToJson());
            }
        }

        [TestMethod]
        public async Task DeleteTestAsync()
        {
            List<IDatabaseObject> listToRemove = new List<IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertItems = new Dictionary<IDatabaseObject, IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertOutput = new Dictionary<IDatabaseObject, IDatabaseObject>();
            //Adding items and updating them
            List<IDatabaseObject> listOfExpected = new List<IDatabaseObject>();
            for (int i = 0; i < 3; i++)
            {
                IDatabaseObject ms = testItems.GetItem();
                IDatabaseObject msCreated = await database.CreateAsync(ms);
                ms.iD = msCreated.iD;
                listToRemove.Add(ms);
                assertOutput.Add(ms, await msCreated.DeleteAsync(database));
                ms.isDeleted = true;
                listOfExpected.Add(ms);
            }
            //Geting assert items from readAsync
            foreach (IDatabaseObject actual in await database.ReadAsync())
            {
                foreach (IDatabaseObject expected in listOfExpected.ToArray())
                {
                    if (actual.ToJson() == expected.ToJson())
                    {
                        assertItems.Add(expected, actual);

                    }
                }
            }
            //Cleaning up database
            foreach (var ms in listToRemove)
            {
                if (database.settings.typeOfDatabase.Equals(DatabaseTypes.InMemoryDatabase))
                {
                }
                else if (database.settings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
                {
                    MongoDatabase<IDatabaseObject> db = (MongoDatabase<IDatabaseObject>)database;
                    ms.isDeleted = true;
                    await db.RemoveAsync(ms);
                }
                else if (database.settings.typeOfDatabase.Equals(DatabaseTypes.MySqlDatabase))
                {

                }
            }
            foreach (var expected in listOfExpected)
            {
                IDatabaseObject actual;
                //Testing the item read from the database useing readAsync
                assertItems.TryGetValue(expected, out actual);
                Assert.AreEqual(expected.ToJson(), actual.ToJson());
                //Testing the item returned from update method
                assertOutput.TryGetValue(expected, out actual);
                Assert.AreEqual(expected.ToJson(), actual.ToJson());
            }
        }

        [TestMethod]
        public void ToJsonTest()
        {
            Log.Logger.Information("Start of ToJsonTest");
            //add custom object here
            IDatabaseObject microservice = new DataBaseObject()
            {
                timeCreated = 636203331300000000,
                iD = "ID",
                isDeleted = false
            };
            string json = microservice.ToJson();
            json.Replace("\n", string.Empty);
            Log.Logger.Information($"Result: {json}");
            var testCase = resourceLoader.GetEmbeddedResourceString(this.GetType().Assembly, "DatabaseObject.json");
            IDatabaseObject testService = JsonConvert.DeserializeObject<DataBaseObject>(testCase);
            Log.Logger.Information($"TestCase: {testService.ToJson()}");
            if (testService.ToJson().Equals(json))
            {
                Log.Logger.Information("Test Result: True");
            }
            else
            {
                Log.Logger.Information("Test Result: False");
            }
            Assert.AreEqual(testService.ToJson(), json);
            Log.Logger.Information("\n");
        }
    }
}
