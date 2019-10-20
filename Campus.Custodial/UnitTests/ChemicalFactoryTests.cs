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
                id = "1",
                name = "One",
                DB = DB
            };
            DB.CreateChemical(chemOne);
            ChemicalFactory ChemFactory = new ChemicalFactory(DB);
            Assert.Equal(ChemFactory.ReadChemical("1").ToString(), chemOne.ToString());

        }

        [Fact]
        public void CreateChemicalTest()
        {
            IDatabase DB = new InMemoryDatabase();
            ChemicalFactory ChemFactory = new ChemicalFactory(DB);
            IChemical created = ChemFactory.CreateChemical("1", "One");
            Assert.Equal("1", created.getID());
            Assert.Equal("One", created.getName());
        }
    }
}
