using Custodial.BoilerPlate.Core.Database;
using Custodial.BoilerPlate.Core.Interfaces;
using Custodial.BoilerPlate.Core.Service_Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Utility;

namespace UnitTests
{
    [TestClass]
    public class AddressingControllerTests
    {
        private IServiceSettings settings = new ServiceSettings();
        private UtilityContainerList testItems = new UtilityContainerList(10);
        private IDatabase database;
        private ServiceController controller;

        public AddressingControllerTests()
        {
            //Set Service Controller;
            settings.InitServiceSettingsAsync("ServiceSettings.json", this.GetType().Assembly);
            controller = new ServiceController();
            database = controller.database;
        }

        [TestMethod]
        public async Task PostTestAsync()
        {
            List<IDatabaseObject> listToRemove = new List<IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertItems = new Dictionary<IDatabaseObject, IDatabaseObject>();
            Dictionary<IDatabaseObject, IDatabaseObject> assertOutput = new Dictionary<IDatabaseObject, IDatabaseObject>();
            //Adding items and updating them
            List<IDatabaseObject> listOfExpected = new List<IDatabaseObject>();
            for (int i = 0; i < 3; i++)
            {
                IDatabaseObject ms = testItems.GetItem();
                IDatabaseObject msCreated = JsonConvert.DeserializeObject<DataBaseObject>(await controller.PostAsync((DataBaseObject)(IDatabaseObject)ms));
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
        public async Task GetFilteredTestAsync()
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

            foreach (IDatabaseObject toRemove in listToRemove)
            {
                IDatabaseObject actual = JsonConvert.DeserializeObject<DataBaseObject>(await controller.GetAsync((DataBaseObject)(IDatabaseObject)toRemove));
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
                    database = new InMemoryDatabase<IDatabaseObject>(settings.databaseSettings)
                    {
                        settings = settings.databaseSettings
                    };
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
                assertItems.TryGetValue(expected, out actual);
                Assert.AreEqual(expected.ToJson(), actual.ToJson());
            }
        }

        [TestMethod]
        public async Task GetAllTestAsync()
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

            foreach (IDatabaseObject actual in JsonConvert.DeserializeObject<List<DataBaseObject>>(await controller.GetAsync()))
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
                    database = new InMemoryDatabase<IDatabaseObject>(settings.databaseSettings)
                    {
                        settings = settings.databaseSettings
                    };

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
                IDatabaseObject ms = testItems.GetItem();
                IDatabaseObject msCreated = await database.CreateAsync(ms);
                ms.iD = msCreated.iD;
                listToRemove.Add(ms);
                assertOutput.Add(ms, JsonConvert.DeserializeObject<DataBaseObject>(await controller.DeleteAsync((DataBaseObject)(IDatabaseObject)ms)));
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
                    database = new InMemoryDatabase<IDatabaseObject>(settings.databaseSettings)
                    {
                        settings = settings.databaseSettings
                    };
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
                assertOutput.Add(msUpdated, JsonConvert.DeserializeObject<DataBaseObject>(await controller.PutAsync(new List<DataBaseObject>()
                {
                    (DataBaseObject)(IDatabaseObject)ms, (DataBaseObject)(IDatabaseObject)msUpdated
                })));
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
                    database = new InMemoryDatabase<IDatabaseObject>(settings.databaseSettings)
                    {
                        settings = settings.databaseSettings
                    };
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
    }
}
