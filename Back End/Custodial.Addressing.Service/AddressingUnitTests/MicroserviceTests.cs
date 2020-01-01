using AddressingUnitTests.Utility;
using Custodial.Addressing.Service;
using Custodial.Addressing.Service.Databases;
using Custodial.Addressing.Service.Interfaces;
using Custodial.Addressing.Service.Service_Settings;
using Custodial.Addressing.Service.Service_Settings.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AddressingUnitTests
{
    [TestClass]
    public class MicroserviceTests
    {
        private IServiceSettings settings = new ServiceSettings();
        private IResourceLoader resourceLoader = new ResourceLoader();
        private UtilityContainerList testItems = new UtilityContainerList(10);
        private IDatabase database = new InMemoryDatabase();

        public MicroserviceTests()
        {
            settings.InitServiceSettingsAsync(this.GetType().Assembly, "ServiceSettings.json");
            if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.InMemoryDatabase))
            {
                database = new InMemoryDatabase()
                {
                    settings = settings.databaseSettings
                };
            }
            else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
            {
                database = new MongoDatabase(settings.databaseSettings);
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
                Microservice ms = (Microservice)testItems.GetItem();
                Microservice msCreated = (Microservice)await database.CreateAsync(ms);
                ms.iD = msCreated.iD;

                Microservice msUpdated = (Microservice)testItems.GetItem();
                msUpdated.iD = ms.iD;
                listToRemove.Add(msUpdated);
                ms.database = database;
                assertOutput.Add(msUpdated, await ms.UpdateAsync(msUpdated));
                listOfExpected.Add(msUpdated);
            }
            //Geting assert items from readAsync
            foreach (Microservice actual in await database.ReadAsync())
            {
                foreach (Microservice expected in listOfExpected.ToArray())
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
                    MongoDatabase db = (MongoDatabase)database;
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
                Microservice ms = (Microservice)testItems.GetItem();
                Microservice msCreated = (Microservice)await database.CreateAsync(ms);
                ms.iD = msCreated.iD;
                listToRemove.Add(ms);
                assertOutput.Add(ms, await msCreated.DeleteAsync(database));
                ms.isDeleted = true;
                listOfExpected.Add(ms);
            }
            //Geting assert items from readAsync
            foreach (Microservice actual in await database.ReadAsync())
            {
                foreach (Microservice expected in listOfExpected.ToArray())
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
                    MongoDatabase db = (MongoDatabase)database;
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
            Microservice microservice = new Microservice()
            {
                timeCreated = new DateTime(2017, 1, 18, 10, 45, 30, DateTimeKind.Utc).Ticks,
                iD = "ID",
                isDeleted = false,
                serviceName = "TestService",
                settings = new ServiceSettings()
                {
                    databaseSettings = new DatabaseSettings()
                    {
                        port = "1111",
                        address = "10.0.0.1",
                        databaseItems = new List<DatabaseCollection>()
                        {
                            new DatabaseCollection()
                            {
                                collectionNames = new List<string>()
                                {

                                },
                                databaseName = ""
                            }
                        }
                    },
                    networkSettings = new NetworkSettings()
                    {
                        ports = new List<string>()
                        {
                            "1111", "2222"
                        },
                        addresses = new List<string>()
                        {
                            "11.0.0.22"
                        }
                    }
                },
                discription = "Discription of service",
                shortName = "Test"
            };
            string json = microservice.ToJson();
            json.Replace("\n", string.Empty);
            Log.Logger.Information($"Result: {json}");
            var testCase = resourceLoader.GetEmbeddedResourceString(this.GetType().Assembly, "TestMicroservice.json");
            Microservice testService = JsonConvert.DeserializeObject<Microservice>(testCase);
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
