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
        IDatabase db = new MongoDatabase();

        [Fact]
        public async System.Threading.Tasks.Task CreateGetTestAsync()
        { 
            IChemical chemOne = util.createChemical($"Bleach", 1, db);
            var resultChemOne = await db.CreateChemicalAsync(chemOne);
            IChemical resultChemOneFinal = util.findChemFromResult(chemOne.ToJson(), resultChemOne);
            Assert.Equal(chemOne.ToJson(), resultChemOneFinal.ToJson());

            IChemical chemTwo = util.createChemical($"Clorax Bleach", 1, db);
            var resultChemTwo = await db.CreateChemicalAsync(chemTwo);
            IChemical resultChemTwoFinal = util.findChemFromResult(chemTwo.ToJson(), resultChemTwo);
            Assert.Equal(chemTwo.ToJson(), resultChemTwoFinal.ToJson());

            IChemical chemThree = util.createChemical($"Agent Orange", 1, db);
            var resultChemThree = await db.CreateChemicalAsync(chemThree);
            IChemical resultChemThreeFinal = util.findChemFromResult(chemThree.ToJson(), resultChemThree);
            Assert.Equal(chemThree.ToJson(), resultChemThreeFinal.ToJson());

            //Clean up Database
            await db.RemoveChemicalAsync(resultChemOneFinal);
            await db.RemoveChemicalAsync(resultChemTwoFinal);
            await db.RemoveChemicalAsync(resultChemThreeFinal);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateUpdateTestAsync()
        {
            //Creation
            IChemical chemOne = util.createChemical($"Bleach", 1, db);
            var resultChemOne = await db.CreateChemicalAsync(chemOne);
            IChemical resultChemOneFinal = util.findChemFromResult(chemOne.ToJson(), resultChemOne);
            Assert.Equal(chemOne.ToJson(), resultChemOneFinal.ToJson());

            IChemical chemTwo = util.createChemical($"Clorax Bleach", 1, db);
            var resultChemTwo = await db.CreateChemicalAsync(chemTwo);
            IChemical resultChemTwoFinal = util.findChemFromResult(chemTwo.ToJson(), resultChemTwo);
            Assert.Equal(chemTwo.ToJson(), resultChemTwoFinal.ToJson());

            IChemical chemThree = util.createChemical($"Agent Orange", 1, db);
            var resultChemThree = await db.CreateChemicalAsync(chemThree);
            IChemical resultChemThreeFinal = util.findChemFromResult(chemThree.ToJson(), resultChemThree);
            Assert.Equal(chemThree.ToJson(), resultChemThreeFinal.ToJson());

            //Update
            IChemical chemOneUpdate = util.createChemical($"Bleach Updated", 1, db);
            var resultChemOneUpdate = await db.UpdateChemical(chemOneUpdate, resultChemOneFinal);
            IChemical resultChemOneFinalUpdate = util.findChemFromResult(chemOneUpdate.ToJson(), resultChemOneUpdate);
            Assert.Equal(chemOneUpdate.ToJson(), resultChemOneFinalUpdate.ToJson());

            IChemical chemTwoUpdate = util.createChemical($"Clorax Bleach Updated", 1, db);
            var resultChemTwoUpdate = await db.UpdateChemical(chemTwoUpdate, resultChemTwoFinal);
            IChemical resultChemTwoFinalUpdate = util.findChemFromResult(chemTwoUpdate.ToJson(), resultChemTwoUpdate);
            Assert.Equal(chemTwoUpdate.ToJson(), resultChemTwoFinalUpdate.ToJson());

            IChemical chemThreeUpdate = util.createChemical($"Agent Orange Updated", 1, db);
            var resultChemThreeUpdate = await db.UpdateChemical(chemThreeUpdate, resultChemThreeFinal);
            IChemical resultChemThreeFinalUpdate = util.findChemFromResult(chemThreeUpdate.ToJson(), resultChemThreeUpdate);
            Assert.Equal(chemThreeUpdate.ToJson(), resultChemThreeFinalUpdate.ToJson());

            //Clean up Database
            await db.RemoveChemicalAsync(resultChemOneFinalUpdate);
            await db.RemoveChemicalAsync(resultChemTwoFinalUpdate);
            await db.RemoveChemicalAsync(resultChemThreeFinalUpdate);
        }
    }
}
