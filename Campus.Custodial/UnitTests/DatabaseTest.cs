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
        public void CreateGetTest()
        { 
            IDatabase DB = new InMemoryDatabase();
            Chemical chemOne = util.createChemical($"Chemical One", 1, DB);
            DB.CreateChemical(chemOne);
            Assert.NotNull(DB.ReadChemical(chemOne.name));
            IChemical result = DB.ReadChemical(chemOne.name);
            Assert.Equal(result.ToJson(), chemOne.ToJson());
        }

        [Fact]
        public void CreateUpdateTest()
        {
            IDatabase DB = new InMemoryDatabase();
            Chemical chemOne = util.createChemical($"Chemical Generic", 1, DB);
            Chemical chemOneUpdated = util.createChemical($"Chemical Updated", 1, DB);
            DB.CreateChemical(chemOne);
            Assert.NotNull(DB.ReadChemical(chemOne.name));
            Assert.Equal(DB.ReadChemical(chemOne.name).ToJson(), chemOne.ToJson());
            DB.UpdateChemical(chemOneUpdated, chemOne.name);
            Assert.Null(DB.ReadChemical(chemOne.name).ToString());
            Assert.Equal(DB.ReadChemical(chemOneUpdated.name).ToJson(), chemOneUpdated.ToJson());
        }
    }
}
