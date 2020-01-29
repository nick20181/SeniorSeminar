using Custodial.BoilerPlate;
using Custodial.BoilerPlate.Database;
using Custodial.BoilerPlate.Service_Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Utility;

namespace UnitTests
{
    [TestClass]
    public class DatabaseTests
    {
        private IServiceSettings settings = new ServiceSettings();
        private UtilityContainerList testItems = new UtilityContainerList(10);
        private IDatabase database;

        public DatabaseTests()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs\\DatabaseTests")
                .CreateLogger();
            Log.Logger.Information("\n");
            settings.InitServiceSettingsAsync(this.GetType().Assembly, "ServiceSettings.json");
            if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.InMemoryDatabase))
            {
                database = new InMemoryDatabase<DataBaseObject>()
                {
                    settings = settings.databaseSettings
                };
            }
            else if (settings.databaseSettings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
            {
                database = new MongoDatabase<DataBaseObject>(settings.databaseSettings);
            }
            else
            {
                database = new MySQLDatabase();
            }
        }

        [TestMethod]
        public async Task CreateTestAsync()
        {
            List<IDatabaseObject> listToRemove = new List<IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertItems = new Dictionary<IDatabaseObject, IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertOutput = new Dictionary<IDatabaseObject, IDatabaseObject>();
            //Adding items and updating them
            List<IDatabaseObject> listOfExpected = new List<IDatabaseObject>();
            for (int i = 0; i < 3; i++)
            {
                IDatabaseObject ms = (DataBaseObject)testItems.GetItem();
                IDatabaseObject msCreated = (DataBaseObject)await database.CreateAsync(ms);
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
        public async Task ReadTestAsync()
        {
            List<IDatabaseObject> listToRemove = new List<IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertItems = new Dictionary<IDatabaseObject, IDatabaseObject>();

            List<IDatabaseObject> listOfExpected = new List<IDatabaseObject>();
            for (int i = 0; i < 3; i++)
            {
                IDatabaseObject ms = (DataBaseObject)testItems.GetItem();
                IDatabaseObject msCreated = (DataBaseObject)await database.CreateAsync(ms);
                ms.iD = msCreated.iD;
                ms.timeCreated = msCreated.timeCreated;
                listToRemove.Add(ms);
                listOfExpected.Add(ms);
            }

            foreach (IDatabaseObject toRemove in listToRemove)
            {
                foreach (IDatabaseObject actual in await database.ReadAsync(toRemove))
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
                    database = new InMemoryDatabase<DataBaseObject>()
                    {
                        settings = settings.databaseSettings
                    };
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
        public async Task DeleteTestAsync()
        {
            List<IDatabaseObject> listToRemove = new List<IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertItems = new Dictionary<IDatabaseObject, IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertOutput = new Dictionary<IDatabaseObject, IDatabaseObject>();
            //Adding items and updating them
            List<IDatabaseObject> listOfExpected = new List<IDatabaseObject>();
            for (int i = 0; i < 3; i++)
            {
                IDatabaseObject ms = (DataBaseObject)testItems.GetItem();
                IDatabaseObject msCreated = (DataBaseObject)await database.CreateAsync(ms);
                ms.iD = msCreated.iD;
                listToRemove.Add(ms);
                assertOutput.Add(ms, await database.DeleteAsync(ms));
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
                    database = new InMemoryDatabase<DataBaseObject>()
                    {
                        settings = settings.databaseSettings
                    };
                }
                else if (database.settings.typeOfDatabase.Equals(DatabaseTypes.MongoDatabase))
                {
                    MongoDatabase<DataBaseObject> db = (MongoDatabase<DataBaseObject>)database;
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
        public async Task UpdateTestAsync()
        {
            List<IDatabaseObject> listToRemove = new List<IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertItems = new Dictionary<IDatabaseObject, IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertOutput = new Dictionary<IDatabaseObject, IDatabaseObject>();
            //Adding items and updating them
            List<IDatabaseObject> listOfExpected = new List<IDatabaseObject>();
            for (int i = 0; i < 3; i++)
            {
                IDatabaseObject ms = (DataBaseObject)testItems.GetItem();
                IDatabaseObject msCreated = (DataBaseObject)await database.CreateAsync(ms);
                ms.iD = msCreated.iD;

                IDatabaseObject msUpdated = (DataBaseObject)testItems.GetItem();
                msUpdated.iD = ms.iD;
                listToRemove.Add(msUpdated);
                assertOutput.Add(msUpdated, await database.UpdateAsync(ms, msUpdated));
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
                    database = new InMemoryDatabase<DataBaseObject>()
                    {
                        settings = settings.databaseSettings
                    };
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
                IDatabaseObject msCreated = await database.CreateAsync(ms);
                ms.iD = msCreated.iD;
                ms.timeCreated = msCreated.timeCreated;
                listToRemove.Add(ms);
                listOfExpected.Add(ms);
            }

            foreach (IDatabaseObject actual in await database.ReadAsync())
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
                    database = new InMemoryDatabase<DataBaseObject>()
                    {
                        settings = settings.databaseSettings
                    };

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
