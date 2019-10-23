using Campus.Custodial.Chemicals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using static Campus.Custodial.Chemicals.Chemical;

namespace UnitTests
{
    public class ChemicalTests
    {
        private Utility util = new Utility();

        [Fact]
        public void UpdateChemicalTest()
        {
            IDatabase DB = new InMemoryDatabase();
            Chemical chemOne = util.createChemical($"Generic Chemical", 1, DB);
            Chemical chemOneUpdated = util.createChemical($"Generic Chemical Updated", 1, DB);
            chemOne.UpdateChemical(chemOneUpdated);
            Assert.Equal(chemOne.ToJson(), chemOneUpdated.ToJson());
        }

        [Fact]
        public void DeleteChemicalTest()
        {
            IDatabase DB = new InMemoryDatabase();
            Chemical chemOne = util.createChemical($"Generic Chemical", 1, DB);
            chemOne.DeleteChemical();
            Assert.True(chemOne.GetDeletedStatus());
        }
    }
}
