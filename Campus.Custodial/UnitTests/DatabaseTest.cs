using Xunit;
using Campus.Custodial.Chemicals;
using System.Collections.Generic;
using static Campus.Custodial.Chemicals.Chemical;
using Campus.Custodial.Chemicals.Interfaces;
using System;
using System.Linq;

namespace UnitTests
{
    public class DatabaseTest
    {
        private Utility util = new Utility();

        [Fact]
        public async System.Threading.Tasks.Task CreateGetTestAsync()
        { 
            IDatabase DB = new MongoDatabase();
            Chemical chemOne = util.createChemical($"Chemical One", 1, DB);
            await DB.CreateChemicalAsync(chemOne);
            Assert.NotNull(await DB.ReadChemicalAsync(chemOne.chemicalName));
            List<IChemical> result = await DB.ReadChemicalAsync(chemOne.chemicalName);
            List<IChemical> temp = new List<IChemical>();
            foreach(var x in result)
            {
                if (chemOne.ToJson().Equals(x.ToJson()))
                {
                    Assert.Equal(chemOne.ToJson(), x.ToJson());
                    temp.Add(x);
                } else
                {
                    result.Remove(x);
                }
                
            }
            Assert.True(temp.Count != 0, $"Count of the results that matched the requested Chemical was {temp.Count}" );
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateUpdateTestAsync()
        {
            IDatabase DB = new MongoDatabase();
            Chemical chemOne = util.createChemical($"Chemical Generic", 1, DB);
            Chemical chemOneUpdated = util.createChemical($"Chemical Updated", 1, DB);
            await DB.CreateChemicalAsync(chemOne);
            Assert.NotNull(DB.ReadChemicalAsync(chemOne.chemicalName));

            List<IChemical> result = await DB.ReadChemicalAsync(chemOne.chemicalName);
            List<IChemical> temp = new List<IChemical>();
            foreach (var x in result)
            {
                if (chemOne.ToJson().Equals(x.ToJson()))
                {
                    Assert.Equal(chemOne.ToJson(), x.ToJson());
                    temp.Add(x);
                }
                else
                {
                    result.Remove(x);
                }
            }
            Assert.True(result.Count != 0, $"Count of the results that matched the requested Chemical was {temp.Count}");
            DB.UpdateChemical(chemOneUpdated, chemOne.chemicalName);

            result = await DB.ReadChemicalAsync(chemOne.GetName());
            temp = new List<IChemical>();
            foreach (var x in result.ToArray())
            {
                if (chemOne.ToJson().Equals(x.ToJson()))
                {
                    Assert.Equal(chemOne.ToJson(), x.ToJson());
                    temp.Add(x);
                }
                else
                {
                    result.Remove(x);
                }
            }
            Assert.True(temp.Count == 0, $"Count of returns are{(await DB.ReadAllChemicalAsync()).Count}");

            List<IChemical> resultUpdated = await DB.ReadChemicalAsync(chemOne.chemicalName);
            List<IChemical> tempUpdated = new List<IChemical>();
            foreach (var x in resultUpdated.ToArray())
            {
                if (chemOneUpdated.ToJson().Equals(x.ToJson()))
                {
                    Assert.Equal(chemOneUpdated.ToJson(), x.ToJson());
                    tempUpdated.Add(x);
                }
                else
                {
                    resultUpdated.Remove(x);
                }

            }
            Assert.True(tempUpdated.Count != 1, $"Count of the results that matched the requested Chemical was {tempUpdated.Count}");
        }
    }
}
