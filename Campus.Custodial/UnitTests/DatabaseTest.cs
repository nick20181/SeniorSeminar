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
                id = "1",
                name = "ChemicalGeneric",
                DB = DB
            };
            DB.CreateChemical(chemOne);
            Assert.NotNull(DB.ReadChemical(chemOne.id));
            Assert.Equal(DB.ReadChemical(chemOne.id).ToString(), chemOne.ToString());
        }

        [Fact]
        public void CreateDeleteTest()
        {
            IDatabase DB = new InMemoryDatabase();
            Chemical chemOne = new Chemical()
            {
                id = "1",
                name = "ChemicalGeneric",
                DB = DB
            };
            DB.CreateChemical(chemOne);
            Assert.NotNull(DB.ReadChemical(chemOne.id));
            Assert.Equal(DB.ReadChemical(chemOne.id).ToString(), chemOne.ToString());
            DB.DeleteChemical(chemOne);
            Assert.Null(DB.ReadChemical(chemOne.id));
        }

        [Fact]
        public void CreateUpdateTest()
        {
            IDatabase DB = new InMemoryDatabase();
            Chemical chemOne = new Chemical()
            {
                id = "1",
                name = "ChemicalGeneric",
                DB = DB
            };
            Chemical chemOneUpdated = new Chemical()
            {
                id = chemOne.id,
                name = "Chemical Updated",
                DB = chemOne.DB
            };
            DB.CreateChemical(chemOne);
            Assert.NotNull(DB.ReadChemical(chemOne.id));
            Assert.Equal(DB.ReadChemical(chemOne.id).ToString(), chemOne.ToString());
            DB.UpdateChemical(chemOneUpdated, chemOne.id);
            Assert.Equal(DB.ReadChemical(chemOne.id).ToString(), chemOneUpdated.ToString());
        }
    }
}
