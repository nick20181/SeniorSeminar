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
    public class MicroserviceFactoryTests
    {
        private UtilityContainerList testItems = new UtilityContainerList(10);
        private IDatabase database = new InMemoryDatabase();
        private IDatabaseObjectFactory factory;

        public MicroserviceFactoryTests()
        {
            factory = new MicroserviceFactory()
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
