using Custodial.BoilerPlate.Service_Settings;
using Custodial.BoilerPlate.Service_Settings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace UnitTests
{
    [TestClass]
    public class SettingsTests
    {
        private IResourceLoader resourceLoader = new ResourceLoader();

        [TestMethod]
        public async System.Threading.Tasks.Task ServiceSettingsTestsAsync()
        {
            IServiceSettings serviceSettings = new ServiceSettings();
            await serviceSettings.InitServiceSettingsAsync(this.GetType().Assembly, "ServiceSettingsTest.json");

            string embeddedString = resourceLoader.GetEmbeddedResourceString(this.GetType().Assembly, "ServiceSettingsTest.json");
            var testCase = JsonConvert.DeserializeObject<ServiceSettings>(embeddedString);
            Assert.AreEqual(serviceSettings.databaseSettings.address, testCase.databaseSettings.address);
            Assert.AreEqual(serviceSettings.databaseSettings.port, testCase.databaseSettings.port);
            Assert.IsTrue(CompareList(serviceSettings.databaseSettings.collectionNames, testCase.databaseSettings.collectionNames));
            Assert.IsTrue(CompareList(serviceSettings.databaseSettings.databaseNames, testCase.databaseSettings.databaseNames));
            Assert.IsTrue(CompareList(serviceSettings.networkSettings.ports, testCase.networkSettings.ports));
            var addresses = new List<string>();
            foreach(var address in await Dns.GetHostAddressesAsync(Dns.GetHostName()))
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    addresses.Add(address.ToString());
                }
            }
            addresses.Add($"localhost");
            testCase.networkSettings.addresses.AddRange(addresses);
            Assert.IsTrue(CompareList(serviceSettings.networkSettings.addresses, testCase.networkSettings.addresses));
        }

        public bool CompareList(List<string> one, List<string> two)
        {
            List<string> results = new List<string>();
            foreach (var x in one)
            {
                results.Add(x);
            }
            foreach (var x in one)
            {
                foreach (var y in two)
                {
                    if (x.Equals(y))
                    {
                        results.Remove(y);
                    }
                }
            }
            if (results.Count == 0)
            {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
