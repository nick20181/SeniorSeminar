using Custodial.Addressing.Service;
using Custodial.Addressing.Service.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class MicroserviceFactoryTest
    {
        private TestContainerList testItems = new TestContainerList(10);
        private IDatabase database = new InMemoryDatabase();
        private IDatabaseObjectFactory factory;
        public MicroserviceFactoryTest()
        {
            factory = new MicroserviceFactory()
            {
                db = database
            };
        }
        [TestMethod]
        public async Task CreateTestAsync()
        {
            List<Microservice> addedList = new List<Microservice>();
            for (int i = 0; i < 2; i++)
            {
                Microservice ms = (Microservice)testItems.GetItem();
                Assert.AreEqual(ms.ToJson(), ((Microservice)await factory.CreateAsync(ms)).ToJson());
                addedList.Add(ms);
            }
            foreach (var x in await factory.ReadAllAsync())
            {
                addedList.Remove((Microservice)x);
            }
            Assert.AreEqual(addedList.Count, 0);
        }
        [TestMethod]
        public async Task ReadAllTestAsync()
        {
            List<Microservice> addedList = new List<Microservice>();
            for (int i = 0; i < 2; i++)
            {
                Microservice ms = (Microservice)testItems.GetItem();
                await factory.CreateAsync(ms);
                addedList.Add(ms);
            }
            foreach (var x in await factory.ReadAllAsync())
            {
                addedList.Remove((Microservice)x);
            }
            Assert.AreEqual(addedList.Count, 0);
        }

        [TestMethod]
        public async Task ReadTestAsync()
        {
            List<Microservice> addedList = new List<Microservice>();
            for (int i = 0; i < 2; i++)
            {
                Microservice ms = (Microservice)testItems.GetItem();
                await factory.CreateAsync(ms);
                addedList.Add(ms);
            }
            foreach (var ms in addedList.ToArray())
            {
                bool hasSeen = false;
                foreach (var msr in await factory.ReadFilteredAsync(ms))
                {
                    if (ms.ToJson().Equals(msr.ToJson()))
                    {
                        addedList.Remove(ms);
                        hasSeen = true;
                    }
                }
                Assert.IsTrue(hasSeen);
            }
            Assert.AreEqual(addedList.Count, 0);
        }
    }
}
