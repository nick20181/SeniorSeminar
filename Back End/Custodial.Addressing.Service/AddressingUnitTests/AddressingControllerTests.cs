using AddressingUnitTests.Utility;
using Custodial.Addressing.Service;
using Custodial.Addressing.Service.Controllers;
using Custodial.Addressing.Service.Interfaces;
using Custodial.Addressing.Service.Service_Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AddressingUnitTests
{
    [TestClass]
    public class AddressingControllerTests
    {
        private UtilityContainerList testItems = new UtilityContainerList(10);
        private IServiceSettings settings = new ServiceSettings();

        public AddressingControllerTests()
        {
            settings.InitServiceSettingsAsync(this.GetType().Assembly, "ServiceSettings.json");
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs\\AddressingControllerTests")
                .CreateLogger();
            Log.Logger.Information("\n");
        }

        [TestMethod]
        public async System.Threading.Tasks.Task PostTestAsync()
        {
            AddressingController controller = new AddressingController();
            for (int i = 0; i < 3; i++)
            {
                Microservice toAdd = (Microservice)testItems.GetItem();
                Assert.AreEqual(toAdd.ToJson(), await controller.PostAsync(toAdd));
            }
        }

        [TestMethod]
        public async Task GetFilteredTestAsync()
        {
            List<Microservice> inControllerGet = new List<Microservice>();
            AddressingController controller = new AddressingController();
            for (int i = 0; i < 3; i++)
            {
                Microservice toAdd = (Microservice)testItems.GetItem();
                await controller.PostAsync(toAdd);
                inControllerGet.Add(toAdd);
            }

            foreach (var ms in inControllerGet.ToArray())
            {
                var msr = await controller.GetAsync(ms);
                Assert.AreEqual(ms.ToJson(), msr);
                inControllerGet.Remove(ms);
            }
            Assert.AreEqual(inControllerGet.Count, 0);
        }

        [TestMethod]
        public async Task GetAllTestAsync()
        {
            List<Microservice> inControllerGetAll = new List<Microservice>();
            AddressingController controller = new AddressingController();
            for (int i = 0; i < 3; i++)
            {
                Microservice toAdd = (Microservice)testItems.GetItem();
                await controller.PostAsync(toAdd);
                inControllerGetAll.Add(toAdd);
            }

            string allMs = await controller.GetAsync();
            var temp = allMs.Split(";");
            string[] x = allMs.Split(";");
            foreach (var ms in x)
            {
                if ((!ms.Contains("Start of Microservice list")) && (!ms.Contains("End of Microservice list")) && !String.IsNullOrEmpty(ms))
                {
                    var rms = ms.Replace("\n", "");
                    bool hasSeen = false;
                    foreach (var msl in inControllerGetAll.ToArray())
                    {
                        if (msl.ToJson().Equals(rms))
                        {
                            hasSeen = true;
                            Assert.AreEqual(msl.ToJson(), rms);
                            inControllerGetAll.Remove(msl);
                        }
                    }
                    Assert.IsTrue(hasSeen, $"Has {ms} been seen: {hasSeen}");
                }
            }
            Assert.AreEqual(inControllerGetAll.Count, 0);
        }

        [TestMethod]
        public async Task DeleteTestAsync()
        {
            List<Microservice> inController = new List<Microservice>();
            AddressingController controller = new AddressingController();
            for (int i = 0; i < 3; i++)
            {
                Microservice toAdd = (Microservice)testItems.GetItem();
                await controller.PostAsync(toAdd);
                toAdd.isDeleted = true;
                inController.Add(toAdd);
            }

            foreach (var ms in inController)
            {
                Assert.AreEqual(await controller.DeleteAsync(ms), ms.ToJson());
            }

            foreach (var ms in inController.ToArray())
            {
                Microservice service = JsonConvert.DeserializeObject<Microservice>(await controller.GetAsync(ms));
                Assert.IsTrue(service.isDeleted);
                inController.Remove(ms);
            }
            Assert.AreEqual(inController.Count, 0);
        }

        [TestMethod]
        public async Task UpdateTestAsync()
        {
            AddressingController controller = new AddressingController();
            List<Microservice> inController = new List<Microservice>();
            List<Microservice> inControllerUpdated = new List<Microservice>();

            for (int i = 0; i < 3; i++)
            {
                Microservice toAdd = (Microservice)testItems.GetItem();
                await controller.PostAsync(toAdd);
                toAdd.isDeleted = true;
                inController.Add(toAdd);
            }

            foreach (var ms in inController)
            {
                Microservice toUpdate = (Microservice)testItems.GetItem();
                toUpdate.iD = ms.iD;
                Assert.AreEqual(await controller.PutAsync(ms.iD, toUpdate), toUpdate.ToJson());
                inControllerUpdated.Add(toUpdate);
            }

            foreach (var ms in inControllerUpdated.ToArray())
            {
                Assert.AreEqual(await controller.GetAsync(ms), ms.ToJson());
                inControllerUpdated.Remove(ms);
            }
            Assert.AreEqual(inControllerUpdated.Count, 0);
        }
    }
}
