using AddressingUnitTests.Utility;
using Custodial.Addressing.Service;
using Custodial.Addressing.Service.Databases;
using Custodial.Addressing.Service.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AddressingUnitTests
{
    [TestClass]
    public class DatabaseTests
    {
        private UtilityContainerList testItems = new UtilityContainerList(10);
        private IDatabase database = new InMemoryDatabase();

        public DatabaseTests()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs\\DatabaseTests")
                .CreateLogger();
            Log.Logger.Information("\n");
        }

        [TestMethod]
        public async Task CreateTestAsync()
        {
            Microservice ms = (Microservice)testItems.GetItem();
            Microservice created = (Microservice)await database.CreateAsync(ms);
            Assert.AreEqual(ms.ToJson(), created.ToJson());
            Assert.AreEqual((await database.ReadAsync(ms)).Count, 1);
            foreach (var msRecived in await database.ReadAsync(ms))
            {
                if (msRecived.iD.Equals(ms.iD))
                {
                    Assert.AreEqual(msRecived, ms);
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
            for (int i = 0; i < 3; i++)
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
}
