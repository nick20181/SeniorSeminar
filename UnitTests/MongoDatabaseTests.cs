using Campus.Service.Address;
using Campus.Service.Address.Implementations;
using Campus.Service.Address.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class MongoDatabaseTests : IDisposable
    {
        private IDatabase database;
        private IServiceSettings settings;
        Dictionary<string, MicroService> testValueLookup;

        public MongoDatabaseTests()
        {
            settings = new ServiceSettings();
            settings.intilizeServiceAsync();
            database = new MongoDatabase(settings.databaseSettings);
            testValueLookup = new Dictionary<string, MicroService>();
        }

        [Fact]
        public void MongoCreationTest()
        {
            Assert.NotNull(database);
        }

        [Fact]
        public async Task CreateTestAsync()
        {
            testValueLookup.Add($"CreateTestValue", new MicroService()
            {
                serviceName = "Create.Test.Value",
                shortName = "CTV",
                discription = "Discription of CTV",
                settings = settings
            });
            MicroService value;
            testValueLookup.TryGetValue("CreateTestValue", out value);

            Assert.Equal(value.ToJson(), (await database.CreateAsync(value)).ToJson());
        }

        [Fact]
        public async  Task DeleteTestAsync()
        {
            testValueLookup.Add($"DeleteTestValue", new MicroService()
            {
                serviceName = "Delete.Test.Value",
                shortName = "DTV",
                discription = "Discription of DTV",
                settings = settings
            });
            MicroService value;
            testValueLookup.TryGetValue("DeleteTestValue", out value);
            await database.CreateAsync(value);
            Assert.Equal(value.ToJson(), (await database.DeleteAsync(value)).ToJson());
        }

        [Fact]
        public async Task ReadTestAsync()
        {
            testValueLookup.Add($"ReadTestValue", new MicroService()
            {
                serviceName = "Read.Test.Value",
                shortName = "RTV",
                discription = "Discription of RTV",
                settings = settings
            });
            MicroService value;
            testValueLookup.TryGetValue("ReadTestValue", out value);
            await database.CreateAsync(value);
            foreach (var x in await database.ReadAsync(value))
            {
                if (x.ToJson().Equals(value.ToJson()))
                {
                    Assert.Equal(value.ToJson(), x.ToJson());
                }
            }
        }

        [Fact]
        public async Task UpadteTestAsync()
        {
            testValueLookup.Add($"UpdateTestValueOrginal", new MicroService()
            {
                serviceName = "Update.Test.Value.Orginal",
                shortName = "UTVO",
                discription = "Discription of UTVO",
                settings = settings
            });

            testValueLookup.Add($"UpdateTestValueUpdated", new MicroService()
            {
                serviceName = "Update.Test.Value.Updated",
                shortName = "UTVU",
                discription = "Discription of UTVU",
                settings = settings
            });
            MicroService valueOrginal;
            testValueLookup.TryGetValue("UpdateTestValueOrginal", out valueOrginal);
            await database.CreateAsync(valueOrginal);

            MicroService valueUpdated;
            testValueLookup.TryGetValue("UpdateTestValueUpdated", out valueUpdated);
            Assert.Equal (valueUpdated.ToJson(), (await database.UpdateAsync(valueOrginal, valueUpdated)).ToJson());
        }

        public void Dispose()
        {
            foreach(var val in testValueLookup)
            {
                try
                {
                   database.RemoveAsync(val.Value);
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
