using AddressingUnitTests.Utility;
using Custodial.Addressing.Service;
using Custodial.Addressing.Service.Databases;
using Custodial.Addressing.Service.Service_Settings;
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
        private IResourceLoader resourceLoader = new ResourceLoader();
        private UtilityContainerList testItems = new UtilityContainerList(10);
        private IDatabase database = new InMemoryDatabase();

        public MicroserviceTests()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs\\MicroserviceTests")
                .CreateLogger();
            Log.Logger.Information("\n");

        }

        [TestMethod]
        public async Task UpdateTestAsync()
        {
            List<Microservice> listToCheck = new List<Microservice>();
            for (int i = 0; i < 2; i++)
            {
                Microservice ms = (Microservice)testItems.GetItem();
                Microservice msUpdated = (Microservice)testItems.GetItem();
                msUpdated.iD = ms.iD;
                await database.CreateAsync(ms);
                listToCheck.Add(msUpdated);
                await database.UpdateAsync(ms, msUpdated);
            }
            foreach (var msl in listToCheck.ToArray())
            {
                foreach (var msr in await database.ReadAsync(msl))
                {
                    if (msl.iD.Equals(msr.iD))
                    {
                        listToCheck.Remove((Microservice)msr);
                        Assert.AreEqual(msl.ToJson(), msr.ToJson());
                    }
                }
            }
            Assert.AreEqual(listToCheck.Count, 0);
        }

        [TestMethod]
        public async Task DeleteTestAsync()
        {
            List<Microservice> listToCheck = new List<Microservice>();
            for (int i = 0; i < 2; i++)
            {
                Microservice ms = (Microservice)testItems.GetItem();
                await database.CreateAsync(ms);
                await ms.DeleteAsync(database);
                ms.isDeleted = true;
                listToCheck.Add(ms);
            }
            foreach (var msl in listToCheck.ToArray())
            {
                foreach (var msr in await database.ReadAsync(msl))
                {
                    if (msl.iD.Equals(msr.iD))
                    {
                        listToCheck.Remove(msl);
                        Assert.IsTrue(msr.isDeleted);
                    }
                }
            }
            Assert.AreEqual(listToCheck.ToArray().Length, 0);
        }

        [TestMethod]
        public void ToJsonTest()
        {
            Log.Logger.Information("Start of ToJsonTest");
            Microservice microservice = new Microservice()
            {
                timeCreated = new DateTime(2017, 1, 18, 10, 45, 30, DateTimeKind.Utc),
                iD = "ID",
                isDeleted = false,
                serviceName = "TestService",
                settings = new ServiceSettings()
                {
                    databaseSettings = new DatabaseSettings()
                    {
                        port = "1111",
                        address = "10.0.0.1",
                        collectionNames = new List<string>()
                        {
                            "CollectionOne", "CollectionTwo"
                        },
                        databaseNames = new List<string>()
                        {
                            "DatabaseOne"
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
