using Campus.Custodial.Chemicals;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class ChemicalFactoryTests
    {
        private Utility util = new Utility();
     
        [Fact]
        public async System.Threading.Tasks.Task ReadChemicalTestAsync()
        {
            IDatabase DB = new MongoDatabase();
            Chemical chemOne = util.createChemical($"Generic Chemical", 1, DB);
            await DB.CreateChemicalAsync(chemOne);
            ChemicalFactory ChemFactory = new ChemicalFactory(DB);
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
            Assert.True(temp.Count != 0, $"Count of the results that matched the requested Chemical was {temp.Count}");
            List<IChemical> resultTwo = await DB.ReadChemicalAsync($"");
            List<IChemical> tempTwo = new List<IChemical>();
            foreach (var x in resultTwo.ToArray())
            {
                if (new Chemical().NullChemical().ToJson().Equals(x.ToJson()))
                {
                    Assert.Equal(new Chemical().NullChemical().ToJson(), x.ToJson());
                    tempTwo.Add(x);
                }
                else
                {
                    resultTwo.Remove(x);
                }

            }
            Assert.True(tempTwo.Count != 0, $"Count of the results that matched the requested Chemical was {tempTwo.Count}");
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateChemicalTestAsync()
        {
            IDatabase DB = new MongoDatabase();
            Chemical chemOne = util.createChemical($"Generic Chemical", 1, DB);
            ChemicalFactory ChemFactory = new ChemicalFactory(DB);
            IChemical created = await ChemFactory.CreateChemicalAsync(chemOne.GetName(), chemOne.GetManufacturer(), chemOne.GetProductIdentifier(),
                chemOne.GetSignalWords(), chemOne.GetHazardStatements(), chemOne.GetPrecautionStatements());
            Assert.Equal(chemOne.ToJson(), created.ToJson());
        }


    }
}
