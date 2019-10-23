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
        public void ReadChemicalTest()
        {
            IDatabase DB = new InMemoryDatabase();
            Chemical chemOne = util.createChemical($"Generic Chemical", 1, DB);
            DB.CreateChemical(chemOne);
            ChemicalFactory ChemFactory = new ChemicalFactory(DB);
            Assert.Equal(ChemFactory.ReadChemical(chemOne.GetName()).ToJson(), chemOne.ToJson());
            Assert.Equal(ChemFactory.ReadChemical($"").ToJson(), new Chemical().NullChemical().ToJson());
        }

        [Fact]
        public void CreateChemicalTest()
        {
            IDatabase DB = new InMemoryDatabase();
            Chemical chemOne = util.createChemical($"Generic Chemical", 1, DB);
            ChemicalFactory ChemFactory = new ChemicalFactory(DB);
            IChemical created = ChemFactory.CreateChemical(chemOne.GetName(), chemOne.GetManufacturer(), chemOne.GetProductIdentifier(), 
                chemOne.GetSignalWords(), chemOne.GetHazardStatements(), chemOne.GetPrecautionStatements());
            Assert.Equal(chemOne.ToJson(), created.ToJson());
        }


    }
}
