using Campus.Custodial.Chemicals;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class ChemicalTests
    {
        [Fact]
        public void UpdateChemicalTest()
        {
            IDatabase DB = new InMemoryDatabase();
            Chemical chemOne = new Chemical()
            {
                name = "ChemicalGeneric",
                DB = DB
            };
            Chemical chemOneUpdated = new Chemical()
            {
                name = "Chem One Updated",
                DB = chemOne.DB,
                deleted = chemOne.deleted
            };
            chemOne.UpdateChemical(chemOneUpdated);
            Assert.Equal(chemOne.ToString(), chemOneUpdated.ToString());
        }

        [Fact]
        public void DeleteChemicalTest()
        {
            IDatabase DB = new InMemoryDatabase();
            Chemical chemOne = new Chemical()
            {
                name = "ChemicalGeneric",
                DB = DB
            };
            chemOne.DeleteChemical();
            Assert.True(chemOne.GetDeletedStatus());
        }
    }
}
