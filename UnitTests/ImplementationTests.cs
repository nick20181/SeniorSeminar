using Campus.Service.Address.Implementations;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class ImplementationTests
    {
        [Fact]
        public async Task NetworkSettingsInitTestsAsync()
        {
            NetworkSettings settings = new NetworkSettings();
            await settings.intilizeSettingsAsync();
            Assert.Equal("5000", settings.port);
            Assert.Equal("localhost", settings.databaseAddress);
            Assert.Equal("27017", settings.databasePort);

        }

        [Fact]
        public async Task MicroServiceTestsAsync()
        {
            MicroService test = new MicroService()
            {
                ServiceName = $"Test",
                discription = $"TestDiscription",
                ID = $"001"
            };
            await test.intilizeSettingsAsync();
            MicroService testResult = JsonConvert.DeserializeObject<MicroService>(test.ToJson());
            Assert.Equal(test.ToJson(), testResult.ToJson());
        }
    }
}
