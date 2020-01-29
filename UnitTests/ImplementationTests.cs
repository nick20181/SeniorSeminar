using Campus.Service.Address.Implementations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class ImplementationTests
    {
        [Fact]
        public async Task ServiceSettingsTestsAsync()
        {
            ServiceSettings ServiceSetting = new ServiceSettings()
            {
                networkSettings = new NetworkSettings()
                {
                    port = "500",
                    addresses = new List<string>()
                    {
                        $"10.100.128.135"
                    }
                },
                databaseSettings = new DatabaseSettings() {
                    port = "27017",
                    address = $"localhost",
                    collectionNames = new List<string>()
                    {
                        "Microservices"
                    },
                    databaseName = "ServiceAddressService"
                }
            };
            ServiceSettings toTest = new ServiceSettings();
            await toTest.intilizeServiceAsync();

            Assert.Equal(JsonConvert.SerializeObject(ServiceSetting), JsonConvert.SerializeObject(toTest));
        }

        [Fact]
        public async Task MicroServiceTestsAsync()
        {
            MicroService test = new MicroService()
            {
                serviceName = $"Test",
                discription = $"TestDiscription"
            };
            await test.intilizeServiceAsync();
            MicroService testResult = JsonConvert.DeserializeObject<MicroService>(test.ToJson());
            Assert.Equal(test.ToJson(), testResult.ToJson());
        }
    }
}
