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
        }

        [TestMethod]
        public async Task GetFilteredTestAsync()
        {
        }

        [TestMethod]
        public async Task GetAllTestAsync()
        {
        }

        [TestMethod]
        public async Task DeleteTestAsync()
        {
        }

        [TestMethod]
        public async Task UpdateTestAsync()
        {
        }
    }
}
