using Xunit;
using Campus.Custodial.Chemicals;
namespace UnitTests
{
    public class DatabaseTest
    {
        [Fact]
        public void CreateGetTest()
        {
            IDatabase DB = new InMemoryDatabase();
            Chemical chemOne = new Chemical()
            {
                name = "ChemicalGeneric",
                DB = DB
            };
            DB.CreateChemical(chemOne);
            Assert.NotNull(DB.ReadChemical(chemOne.name));
            Assert.Equal(DB.ReadChemical(chemOne.name).ToString(), chemOne.ToString());
        }

        [Fact]
        public void CreateUpdateTest()
        {
            IDatabase DB = new InMemoryDatabase();
            Chemical chemOne = new Chemical()
            {
                name = "ChemicalGeneric",
                DB = DB
            };
            Chemical chemOneUpdated = new Chemical()
            {
                name = "Chemical Updated",
                DB = chemOne.DB
            };
            DB.CreateChemical(chemOne);
            Assert.NotNull(DB.ReadChemical(chemOne.name));
            Assert.Equal(DB.ReadChemical(chemOne.name).ToString(), chemOne.ToString());
            DB.UpdateChemical(chemOneUpdated, chemOne.name);
            Assert.Null(DB.ReadChemical($"ChemicalGeneric").ToString());
            Assert.Equal(DB.ReadChemical(chemOneUpdated.name).ToString(), chemOneUpdated.ToString());
        }
    }
}
