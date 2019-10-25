using Campus.Custodial.Chemicals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Campus.Custodial.Chemicals.Chemical;

namespace UnitTests
{
    public class ChemicalTests
    {
        private Utility util = new Utility();
        private static IDatabase db = new MongoDatabase();
        IChemicalFactory chemFactory = new ChemicalFactory(db);

        [Fact]
        public async Task UpdateChemicalTestAsync()
        {
            IChemical chemOne = util.createChemical($"Bleach", 1, db);
            IChemical chemTwo = util.createChemical($"Clorax Bleach", 1, db);
            IChemical chemThree = util.createChemical($"Agent Orange", 1, db);
            List<IChemical> temp = new List<IChemical>()
            {
                chemOne, chemTwo, chemThree
            };

            await chemFactory.CreateChemicalAsync(chemOne);
            await chemFactory.CreateChemicalAsync(chemTwo);
            await chemFactory.CreateChemicalAsync(chemThree);

            foreach (var x in temp.ToArray())
            {
                foreach (var y in await chemFactory.ReadAllChemicalsAsync())
                {
                    if (x.GetName().Equals(y.GetName()))
                    {
                        temp.Remove(x);
                    }
                }
            }
            Assert.True(temp.Count == 0);

            IChemical chemOneUpdate = util.createChemical($"Bleach Updated", 1, db);
            IChemical chemTwoUpdate = util.createChemical($"Clorax Bleach Updated", 1, db);
            IChemical chemThreeUpdate = util.createChemical($"Agent Orange Updated", 1, db);
            List<IChemical> tempUpdated = new List<IChemical>()
            {
                chemOneUpdate, chemTwoUpdate, chemThreeUpdate
            };

            await chemOne.UpdateChemicalAsync(chemOneUpdate);
            await chemTwo.UpdateChemicalAsync(chemTwoUpdate);
            await chemThree.UpdateChemicalAsync(chemThreeUpdate);

            foreach (var x in tempUpdated.ToArray())
            {
                foreach (var y in await chemFactory.ReadAllChemicalsAsync())
                {
                    if (x.GetName().Equals(y.GetName()))
                    {
                        tempUpdated.Remove(x);
                    }
                }
            }
            Assert.True(tempUpdated.Count == 0);
            await util.DeleteAllDBAsync(db);
        }

        [Fact]
        public async Task DeleteChemicalTestAsync()
        {
            IChemical chemOne = util.createChemical($"Bleach", 1, db);
            IChemical chemTwo = util.createChemical($"Clorax Bleach", 1, db);
            IChemical chemThree = util.createChemical($"Agent Orange", 1, db);
            List<IChemical> temp = new List<IChemical>()
            {
                chemOne, chemTwo, chemThree
            };

            await chemFactory.CreateChemicalAsync(chemOne);
            await chemFactory.CreateChemicalAsync(chemTwo);
            await chemFactory.CreateChemicalAsync(chemThree);

            foreach (var x in temp.ToArray())
            {
                foreach (var y in await chemFactory.ReadAllChemicalsAsync())
                {
                    if (x.GetName().Equals(y.GetName()))
                    {
                        temp.Remove(x);
                    }
                }
            }
            Assert.True(temp.Count == 0);

            //Do Deletes
            await chemOne.DeleteChemicalAsync();
            await chemTwo.DeleteChemicalAsync();
            await chemThree.DeleteChemicalAsync();

            foreach (var x in temp.ToArray())
            {
                foreach (var y in await chemFactory.ReadAllChemicalsAsync())
                {
                    if (x.GetName().Equals(y.GetName()))
                    {
                        if (y.GetDeletedStatus())
                        {
                            temp.Remove(x);
                        }
                    }
                }
            }
            await util.DeleteAllDBAsync(db);
        }
    }
}
