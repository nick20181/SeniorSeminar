using Campus.Service.Address.Implementations;
using Campus.Service.Address.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class MongoDatabaseTests
    {
        private IDatabase database;
        private IServiceSettings settings;

        public MongoDatabaseTests()
        {
            settings = new ServiceSettings();
            settings.intilizeServiceAsync();
            database = new MongoDatabase(settings.databaseSettings);
        }

        [Fact]
        public void MongoCreationTest()
        {
            Assert.NotNull(database);
        }

        [Fact]
        public async Task CreateTestAsync()
        {
            MicroService SAS = new MicroService()
            {
                serviceName = "Service.Address.Service",
                shortName = "SAS",
                discription = "Discription of SAS",
                settings = settings
            };
            Assert.Equal(SAS.ToJson(), (await database.CreateAsync(SAS)).ToJson());
        }

        [Fact]
        public async  Task DeleteTestAsync()
        {
            MicroService SAS = new MicroService()
            {
                serviceName = "Service.Address.Service",
                shortName = "SAS",
                discription = "Discription of SAS",
                settings = settings
            };
            Assert.Equal(SAS.ToJson(), (await database.DeleteAsync(SAS)).ToJson());
        }

        [Fact]
        public async Task ReadTestAsync()
        {
            MicroService SAS = new MicroService()
            {
                serviceName = "Service.Address.Service",
                shortName = "SAS",
                discription = "Discription of SAS",
                settings = settings
            };
            foreach(var x in await database.ReadAsync(SAS))
            {
                if (x.ToJson().Equals(SAS.ToJson()))
                {
                    Assert.Equal(SAS.ToJson(), x.ToJson());
                }
            }
        }

        [Fact]
        public async Task UpadteTestAsync()
        {
            MicroService SAS = new MicroService()
            {
                serviceName = "Service.Address.Service",
                shortName = "SAS",
                discription = "Discription of SAS",
                settings = settings
            };
            MicroService SASU = new MicroService()
            {
                serviceName = "Service.Address.Service.Updated",
                shortName = "SAS",
                discription = "Discription of SAS",
                settings = settings
            };

            Assert.Equal (SASU.ToJson(), (await database.UpdateAsync(SAS, SASU)).ToJson());
        }
    }
}
