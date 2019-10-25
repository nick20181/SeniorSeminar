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
        private static IDatabase db = new MongoDatabase();
        private IChemicalFactory chemFactory = new ChemicalFactory(db);
     
        [Fact]
        public async System.Threading.Tasks.Task ReadChemicalTestAsync()
        {
            IChemical chemOne = util.createChemical($"Bleach", 1, db);
            var resultChemOne = await db.CreateChemicalAsync(chemOne);
            IChemical chemOneFactory = await chemFactory.ReadChemicalAsync(chemOne);
            IChemical resultChemOneFinal = util.findChemFromResult(chemOne.ToJson(), resultChemOne);
            Assert.Equal(chemOne.ToJson(), resultChemOneFinal.ToJson());
            Assert.Equal(chemOne.ToJson(), chemOneFactory.ToJson());

            IChemical chemTwo = util.createChemical($"Clorax Bleach", 1, db);
            var resultChemTwo = await db.CreateChemicalAsync(chemTwo);
            IChemical chemTwoFactory = await chemFactory.ReadChemicalAsync(chemTwo);
            IChemical resultChemTwoFinal = util.findChemFromResult(chemTwo.ToJson(), resultChemTwo);
            Assert.Equal(chemTwo.ToJson(), resultChemTwoFinal.ToJson());
            Assert.Equal(chemTwo.ToJson(), chemTwoFactory.ToJson());

            IChemical chemThree = util.createChemical($"Agent Orange", 1, db);
            var resultChemThree = await db.CreateChemicalAsync(chemThree);
            IChemical chemThreeFactory = await chemFactory.ReadChemicalAsync(chemThree);
            IChemical resultChemThreeFinal = util.findChemFromResult(chemThree.ToJson(), resultChemThree);
            Assert.Equal(chemThree.ToJson(), resultChemThreeFinal.ToJson());
            Assert.Equal(chemThree.ToJson(), chemThreeFactory.ToJson());

            await util.DeleteAllDBAsync(db);
        }

        [Fact]
        public async System.Threading.Tasks.Task ReadAllChemicalTestAsync()
        {
            IChemical chemOne = util.createChemical($"Bleach", 1, db);
            var resultChemOne = await db.CreateChemicalAsync(chemOne);
            IChemical chemOneFactory = await chemFactory.ReadChemicalAsync(chemOne);
            IChemical resultChemOneFinal = util.findChemFromResult(chemOne.ToJson(), resultChemOne);
            Assert.Equal(chemOne.ToJson(), resultChemOneFinal.ToJson());
            Assert.Equal(chemOne.ToJson(), chemOneFactory.ToJson());

            IChemical chemTwo = util.createChemical($"Clorax Bleach", 1, db);
            var resultChemTwo = await db.CreateChemicalAsync(chemTwo);
            IChemical chemTwoFactory = await chemFactory.ReadChemicalAsync(chemTwo);
            IChemical resultChemTwoFinal = util.findChemFromResult(chemTwo.ToJson(), resultChemTwo);
            Assert.Equal(chemTwo.ToJson(), resultChemTwoFinal.ToJson());
            Assert.Equal(chemTwo.ToJson(), chemTwoFactory.ToJson());

            IChemical chemThree = util.createChemical($"Agent Orange", 1, db);
            var resultChemThree = await db.CreateChemicalAsync(chemThree);
            IChemical chemThreeFactory = await chemFactory.ReadChemicalAsync(chemThree);
            IChemical resultChemThreeFinal = util.findChemFromResult(chemThree.ToJson(), resultChemThree);
            Assert.Equal(chemThree.ToJson(), resultChemThreeFinal.ToJson());
            Assert.Equal(chemThree.ToJson(), chemThreeFactory.ToJson());

            List<IChemical> tempTwo = new List<IChemical>();
            List<IChemical> check = new List<IChemical>();
            check.Add(chemOne);
            check.Add(chemTwo);
            check.Add(chemThree);

            List<IChemical> temp = await chemFactory.ReadAllChemicalsAsync();
            foreach (var x in check.ToArray())
            {
                foreach (var y in temp.ToArray())
                {
                    if (x.GetName().Equals(y.GetName()))
                    {
                        tempTwo.Add(y);
                    }
                }
            }
            Assert.True(tempTwo.Count == 3, $"Count: {check.Count}");
            await util.DeleteAllDBAsync(db);

        }

        [Fact]
        public async System.Threading.Tasks.Task CreateChemicalTestAsync()
        {

            IChemical chemOne = util.createChemical($"Bleach", 1, db);
            IChemical resultChemOne = await chemFactory.CreateChemicalAsync(chemOne);
            IChemical chemOneFactory = await chemFactory.ReadChemicalAsync(chemOne);
            Assert.Equal(chemOne.ToJson(), chemOneFactory.ToJson());
            Assert.Equal(chemOne.ToJson(), chemOneFactory.ToJson());

            IChemical chemTwo = util.createChemical($"Clorax Bleach", 1, db);
            IChemical resultChemTwo = await chemFactory.CreateChemicalAsync(chemTwo);
            IChemical chemTwoFactory = await chemFactory.ReadChemicalAsync(chemTwo);
            Assert.Equal(chemTwo.ToJson(), chemTwoFactory.ToJson());
            Assert.Equal(chemTwo.ToJson(), chemTwoFactory.ToJson());

            IChemical chemThree = util.createChemical($"Agent Orange", 1, db);
            IChemical resultChemThree = await chemFactory.CreateChemicalAsync(chemThree);
            IChemical chemThreeFactory = await chemFactory.ReadChemicalAsync(chemThree);
            Assert.Equal(chemThree.ToJson(), chemThreeFactory.ToJson());
            Assert.Equal(chemThree.ToJson(), chemThreeFactory.ToJson());

            await util.DeleteAllDBAsync(db);
        }


    }
}
