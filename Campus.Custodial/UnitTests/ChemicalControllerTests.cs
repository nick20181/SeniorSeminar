using Campus.Custodial.Chemicals;
using Campus.Custodial.Chemicals.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class ChemicalControllerTests
    {
        private Utility util = new Utility();

        public List<string> populateListWith(int size)
        {
            List<string> temp = new List<string>();
            for(int i = 0; i != size; i++)
            {
                temp.Add(RandomString(10));
            }
            return temp;
        }

        [Fact]
        public async System.Threading.Tasks.Task ReadChemicalAsync()
        {
            ChemicalController controller = new ChemicalController();
            List<string> ListOfStrings = populateListWith(10);
            foreach (var x in ListOfStrings)
            {
                await controller.PostAsync(x);
            }
            foreach ( var x in ListOfStrings.ToArray())
            {
                var result = await controller.GetAsync(x);
                List<Chemical> chems = new List<Chemical>();
                foreach (var chem in result)
                {
                    chems.Add(JsonConvert.DeserializeObject<Chemical>(chem));
                }
                Assert.True(chems.Count != 0);
                foreach (var chem in chems.ToArray())
                {
                    if (chem.GetName().Equals(x))
                    {
                        ListOfStrings.Remove(x);
                    }
                }
            }
            Assert.True(ListOfStrings.Count == 0);
            await controller.cleanDataBaseAsync();
        }

        [Fact]
        public async System.Threading.Tasks.Task ReadAllChemicalAsync()
        {
            ChemicalController controller = new ChemicalController();
            List<string> ListOfStrings = populateListWith(10);
            List<string> ListToRemove = new List<string>();
            foreach (var x in ListOfStrings)
            {
                await controller.PostAsync(x);
                ListToRemove.Add(x);
            }

            string json = await controller.GetAllAsync();
            List<Chemical> fromJson = JsonConvert.DeserializeObject<List<Chemical>>(json);
            foreach (var x in fromJson)
            {
                foreach (var y in ListToRemove)
                {
                    if (x.GetName().Equals(y))
                    {
                        Assert.Equal(y, x.GetName());
                        ListOfStrings.Remove(y);
                    }
                }
            }
            if (ListOfStrings.Count != 0)
            {
                Assert.True(false, $"String List is not empty");
            }
            else
            {
                Assert.True(true);
            }
            await controller.cleanDataBaseAsync();
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteChemicalAsync()
        {
            ChemicalController controller = new ChemicalController();
            List<string> ListOfStrings = populateListWith(10);
            List<string> ListToRemove = new List<string>();
            foreach (var x in ListOfStrings)
            {
                await controller.PostAsync(x);
                ListToRemove.Add(x);
                await controller.DeleteAsync(x);
            }
            foreach (var x in ListOfStrings)
            {
                List<string> result = await controller.GetAsync(x);
                List<Chemical> chems = new List<Chemical>();
                foreach (var s in result)
                {
                    chems.Add(JsonConvert.DeserializeObject<Chemical>(s));
                }
                foreach (var s in result.ToArray())
                {
                    foreach (var chem in chems.ToArray())
                    {
                        if (chem.ToJson().Equals(s))
                        {
                            result.Remove(s);
                        }
                    }
                }
                Assert.True(result.Count == 0);
            }
            await controller.cleanDataBaseAsync();
        }

        [Fact]
        public async System.Threading.Tasks.Task PutChemicalAsync()
        {
            ChemicalController controller = new ChemicalController();
            List<string> ListOfStrings = populateListWith(10);
            List<string> ListOfUpdatedStrings = populateListWith(10);
            Dictionary<string, string> updatedStrings = new Dictionary<string, string>();
            var a = ListOfUpdatedStrings.ToArray();
            int i = 0;
            foreach (var x in ListOfStrings)
            {
                updatedStrings.Add(x, a[i]);
                i++;
            }

            List<string> ListToRemove = new List<string>();
            foreach (var x in ListOfStrings)
            {
                string temp;
                updatedStrings.TryGetValue(x, out temp);
                await controller.PostAsync(x);
                ListToRemove.Add(temp);
                await controller.PutAsync(x, temp);
            }
            foreach (var x in ListOfUpdatedStrings)
            {
                List<string> result = await controller.GetAsync(x);
                List<Chemical> chems = new List<Chemical>();
                foreach (var s in result)
                {
                    chems.Add(JsonConvert.DeserializeObject<Chemical>(s));
                }
                foreach (var s in result.ToArray())
                {
                    foreach (var chem in chems.ToArray())
                    {
                        if (chem.ToJson().Equals(s))
                        {
                            result.Remove(s);
                        }
                    }
                }
                Assert.True(result.Count == 0);
            }
            await controller.cleanDataBaseAsync();
        }

        private Random random = new Random();
        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
