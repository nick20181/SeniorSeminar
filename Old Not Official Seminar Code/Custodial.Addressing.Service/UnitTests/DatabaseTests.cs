using Custodial.Addressing.Service;
using Custodial.Addressing.Service.Database;
using Custodial.Addressing.Service.Service_Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class DatabaseTests
    {
        private TestContainerList testItems = new TestContainerList(10);
        private IDatabase database = new InMemoryDatabase();

        [TestMethod]
        public async Task CreateTestAsync()
        {
            Microservice ms = (Microservice) testItems.GetItem();
            Microservice created = (Microservice) await database.CreateAsync(ms);
            Assert.AreEqual(ms.ToJson(), created.ToJson());
            Assert.AreEqual((await database.ReadAsync(ms)).Count, 1);
            foreach (var msRecived in await database.ReadAsync(ms))
            {
                if (msRecived.iD.Equals(ms.iD))
                {
                    Assert.AreEqual(msRecived,ms);
                }
            }

            ms = (Microservice)testItems.GetItem();
            created = (Microservice)await database.CreateAsync(ms);
            Assert.AreEqual(ms.ToJson(), created.ToJson());
            Assert.AreEqual((await database.ReadAsync(ms)).Count, 1);
            foreach (var msRecived in await database.ReadAsync(ms))
            {
                if (msRecived.iD.Equals(ms.iD))
                {
                    Assert.AreEqual(msRecived, ms);
                }
            }
        }

        [TestMethod]
        public async Task ReadTestAsync()
        {

            Microservice ms = (Microservice)testItems.GetItem();
            await database.CreateAsync(ms);
            Assert.AreEqual((await database.ReadAsync(ms)).Count, 1);
            foreach (var msRecived in await database.ReadAsync(ms))
            {
                if (msRecived.iD.Equals(ms.iD))
                {
                    Assert.AreEqual(msRecived, ms);
                }
            }

            ms = (Microservice)testItems.GetItem();
            await database.CreateAsync(ms);
            Assert.AreEqual((await database.ReadAsync(ms)).Count, 1);
            foreach (var msRecived in await database.ReadAsync(ms))
            {
                if (msRecived.iD.Equals(ms.iD))
                {
                    Assert.AreEqual(msRecived.ToJson(), ms.ToJson());
                }
            }
        }

        [TestMethod]
        public async Task DeleteTestAsync()
        {
            Microservice ms = (Microservice)testItems.GetItem();
            await database.CreateAsync(ms);
            await database.DeleteAsync(ms);
            Assert.AreEqual((await database.ReadAsync(ms)).Count, 1);
            foreach (var msRecived in await database.ReadAsync(ms))
            {
                if (msRecived.iD.Equals(ms.iD))
                {
                    Assert.IsTrue(msRecived.isDeleted);
                }
            }

            ms = (Microservice)testItems.GetItem();
            await database.CreateAsync(ms);
            await database.DeleteAsync(ms);
            Assert.AreEqual((await database.ReadAsync(ms)).Count, 1);
            foreach (var msRecived in await database.ReadAsync(ms))
            {
                if (msRecived.iD.Equals(ms.iD))
                {
                    Assert.IsTrue(msRecived.isDeleted);
                }
            }

        }

        [TestMethod]
        public async Task UpdateTestAsync()
        {
            Microservice ms = (Microservice)testItems.GetItem();
            Microservice msUpdated = (Microservice)testItems.GetItem();
            msUpdated.iD = ms.iD;
            await database.CreateAsync(ms);
            await database.UpdateAsync(ms, msUpdated);
            Assert.AreEqual((await database.ReadAsync(ms)).Count, 1);
            foreach (var msRecived in await database.ReadAsync(ms))
            {
                if (msRecived.iD.Equals(ms.iD))
                {
                    Assert.AreEqual(msRecived.ToJson(), msUpdated.ToJson());
                }
            }

            ms = (Microservice)testItems.GetItem();
            msUpdated = (Microservice)testItems.GetItem();
            msUpdated.iD = ms.iD;
            await database.CreateAsync(ms);
            await database.UpdateAsync(ms, msUpdated);
            Assert.AreEqual((await database.ReadAsync(ms)).Count, 1);
            foreach (var msRecived in await database.ReadAsync(ms))
            {
                if (msRecived.iD.Equals(ms.iD))
                {
                    Assert.AreEqual(msRecived.ToJson(), msUpdated.ToJson());
                }
            }
        }

        [TestMethod]
        public async Task ReadAllTestAsync()
        {
            List<IDatabaseObject> listOfReadDatabaseObjects = new List<IDatabaseObject>();
            for( int i = 0; i < 3; i++)
            {
                Microservice ms = (Microservice)testItems.GetItem();
                listOfReadDatabaseObjects.Add(ms);
                await database.CreateAsync(ms);
            }
            Assert.AreEqual((await database.ReadAsync()).Count, 3);
            foreach (var ms in await database.ReadAsync())
            {
                listOfReadDatabaseObjects.Remove(ms);
            }
            Assert.AreEqual(listOfReadDatabaseObjects.Count, 0);
        }
    }

    public class TestContainerList
    {
        public List<TestContainer> testList = new List<TestContainer>();
        private ObjectIDGenerator IDGen = new ObjectIDGenerator();
        public TestContainerList(int countOfItems)
        {
            for(int i = 0; i < countOfItems; i++)
            {
                Random rando = new Random();
                var ms = new Microservice()
                {
                    discription = RandomString(10),
                    database = null,
                    isDeleted = false,
                    serviceName = RandomString(10),
                    settings = new ServiceSettings()
                    {
                        databaseSettings = new DatabaseSettings()
                        {
                            address = RandomString(10),
                            port = RandomString(10),
                            collectionNames = new List<string>()
                            {
                                RandomString(10), RandomString(10)
                            },
                            databaseNames = new List<string>()
                            {
                                RandomString(10)
                            }
                        },
                        networkSettings = new NetworkSettings()
                        {
                            addresses = new List<string>()
                            {
                                RandomString(10), RandomString(10)
                            },
                            ports = new List<string>()
                            {
                                RandomString(10)
                            }
                        }
                    },
                    shortName = RandomString(3),
                    timeCreated = DateTime.UtcNow.AddDays(rando.NextDouble())
                };
                bool hasId = false;
                ms.iD = IDGen.GetId(ms, out hasId).ToString();
                testList.Add(new TestContainer(ms));
            }
        }

        public IDatabaseObject GetItem()
        {
            foreach (var x in testList)
            {
                if (!x.isUsed)
                {
                    x.isUsed = true;
                    return x.service;
                }
            }
            return null;
        }

        private string RandomString(int size, bool lowerCase = true)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }

    public class TestContainer
    {
        public bool isUsed = false;
        public IDatabaseObject service;
        public TestContainer(IDatabaseObject ob)
        {
            service = ob;
        }
    }
}
