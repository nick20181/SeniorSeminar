using Custodial.BoilerPlate.Core;
using Custodial.BoilerPlate.Core.Database;
using Custodial.BoilerPlate.Core.Interfaces;
using Custodial.BoilerPlate.Core.Service_Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTests.Utility;

namespace UnitTests
{
    [TestClass]
    public class DatabaseObjectFactoryTests
    {
        private IServiceSettings settings = new ServiceSettings();
        private UtilityContainerList testItems = new UtilityContainerList(10);
        private IDatabase database;
        private IDatabaseObjectFactory factory;

        public DatabaseObjectFactoryTests()
        {
            settings.InitServiceSettingsAsync("ServiceSettings.json", this.GetType().Assembly);
            if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.InMemoryDatabase))
            {
                database = new InMemoryDatabase<DataBaseObject>(settings.databaseSettings)
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
            factory = new DatabaseObjectFactory<DataBaseObject>()
            {
                db = database
            };
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs\\MicroserviceFactoryTests")
                .CreateLogger();
            Log.Logger.Information("\n");
        }

        [TestMethod]
        public async Task CreateTestAsync()
        {
            List<IDatabaseObject> listToRemove = new List<IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertItems = new Dictionary<IDatabaseObject, IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertOutput = new Dictionary<IDatabaseObject, IDatabaseObject>();
            List<IDatabaseObject> listOfExpected = new List<IDatabaseObject>();
            for (int i = 0; i < 3; i++)
            {
                IDatabaseObject ms = testItems.GetItem();
                IDatabaseObject msCreated = await factory.CreateAsync(ms);
                ms.iD = msCreated.iD;
                listToRemove.Add(ms);
                assertOutput.Add(ms, msCreated);
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
                    MongoDatabase<DataBaseObject> db = (MongoDatabase<DataBaseObject>)database;
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
        public async Task ReadAllTestAsync()
        {
            List<IDatabaseObject> listToRemove = new List<IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertItems = new Dictionary<IDatabaseObject, IDatabaseObject>();

            List<IDatabaseObject> listOfExpected = new List<IDatabaseObject>();
            for (int i = 0; i < 3; i++)
            {
                IDatabaseObject ms = testItems.GetItem();
                IDatabaseObject msCreated = await factory.CreateAsync(ms);
                ms.iD = msCreated.iD;
                ms.timeCreated = msCreated.timeCreated;
                listToRemove.Add(ms);
                listOfExpected.Add(ms);
            }

            foreach (IDatabaseObject actual in await factory.ReadAllAsync())
            {
                foreach (IDatabaseObject expected in listOfExpected.ToArray())
                {
                    if (actual.ToJson().Equals(expected.ToJson()))
                    {
                        assertItems.Add(expected, actual);

                    }
                }
            }

            foreach (var ms in listToRemove)
            {
                if (database.settings.typeOfDatabase.Equals(DatabaseTypes.InMemoryDatabase))
                {

                }
                else if (database.settings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
                {
                    MongoDatabase<DataBaseObject> db = (MongoDatabase<DataBaseObject>)database;
                    await db.RemoveAsync(ms);
                }
                else if (database.settings.typeOfDatabase.Equals(DatabaseTypes.MySqlDatabase))
                {

                }
            }
            foreach (var expected in listOfExpected)
            {
                IDatabaseObject actual;
                assertItems.TryGetValue(expected, out actual);
                Assert.AreEqual(expected.ToJson(), actual.ToJson());
            }
        }

        [TestMethod]
        public async Task ReadTestAsync()
        {

            List<IDatabaseObject> listToRemove = new List<IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertItems = new Dictionary<IDatabaseObject, IDatabaseObject>();

            List<IDatabaseObject> listOfExpected = new List<IDatabaseObject>();
            for (int i = 0; i < 3; i++)
            {
                IDatabaseObject ms = testItems.GetItem();
                IDatabaseObject msCreated = await factory.CreateAsync(ms);
                ms.iD = msCreated.iD;
                ms.timeCreated = msCreated.timeCreated;
                listToRemove.Add(ms);
                listOfExpected.Add(ms);
            }

            foreach (IDatabaseObject toRemove in listToRemove)
            {
                foreach (IDatabaseObject actual in await factory.ReadFilteredAsync(toRemove))
                {
                    foreach (IDatabaseObject expected in listOfExpected.ToArray())
                    {
                        if (actual.ToJson().Equals(expected.ToJson()))
                        {
                            assertItems.Add(expected, actual);

                        }
                    }
                }
            }

            foreach (var ms in listToRemove)
            {
                if (database.settings.typeOfDatabase.Equals(DatabaseTypes.InMemoryDatabase))
                {

                }
                else if (database.settings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
                {
                    MongoDatabase<DataBaseObject> db = (MongoDatabase<DataBaseObject>)database;
                    await db.RemoveAsync(ms);
                }
                else if (database.settings.typeOfDatabase.Equals(DatabaseTypes.MySqlDatabase))
                {

                }
            }
            foreach (var expected in listOfExpected)
            {
                IDatabaseObject actual;
                assertItems.TryGetValue(expected, out actual);
                Assert.AreEqual(expected.ToJson(), actual.ToJson());
            }
        }
    }
}
