using Campus.Custodial.Chemicals;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class ChemicalFactoryTests
    {
        [Fact]
        public void ReadChemicalTest()
        {
            IDatabase DB = new InMemoryDatabase();
            Chemical chemOne = new Chemical()
            {
                name = "One",
                DB = DB
            };
            DB.CreateChemical(chemOne);
            ChemicalFactory ChemFactory = new ChemicalFactory(DB);
            Assert.Equal(ChemFactory.ReadChemical("One").ToString(), chemOne.ToString());
            Assert.Null(ChemFactory.ReadChemical("Two").ToString());
        }

        [Fact]
        public void CreateChemicalTest()
        {
            IDatabase DB = new InMemoryDatabase();
            ChemicalFactory ChemFactory = new ChemicalFactory(DB);
            IChemical created = ChemFactory.CreateChemical("One");
            Assert.Equal("One", created.GetName());
        }
    }
}
