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
        public void ReadChemical()
        {
            ChemicalController controller = new ChemicalController();
            List<string> ListOfStrings = populateListWith(10);
            foreach (var x in ListOfStrings)
            {
                controller.Post(x);
            }
            foreach ( var x in ListOfStrings)
            {
                var result = controller.Get(x);
                Chemical jsonResult = JsonConvert.DeserializeObject<Chemical>(result);
                if (!string.IsNullOrEmpty(result))
                {
                    if (!string.IsNullOrEmpty(jsonResult.GetName()))
                    {
                        Assert.Equal(x, jsonResult.GetName());
                    }
                    else
                    {
                        Assert.True(false, $"{x} jsonResult is {jsonResult.GetName()}");
                    }
                } else {
                    Assert.True(false, $"{x} result is {result}");
                } 
            }
            Assert.Equal(new Chemical().NullChemical().ToJson(), controller.Get("x"));
        }

        [Fact]
        public void ReadAllChemical()
        {
            ChemicalController controller = new ChemicalController();
            List<string> ListOfStrings = populateListWith(10);
            List<string> ListToRemove = new List<string>();
            foreach (var x in ListOfStrings)
            {
                controller.Post(x);
                ListToRemove.Add(x);
            }

            string json = controller.GetAll();
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
        }

        [Fact]
        public void DeleteChemical()
        {
            ChemicalController controller = new ChemicalController();
            List<string> ListOfStrings = populateListWith(10);
            List<string> ListToRemove = new List<string>();
            foreach (var x in ListOfStrings)
            {
                controller.Post(x);
                ListToRemove.Add(x);
                controller.Delete(x);
            }
            foreach ( var x in ListOfStrings)
            {
                string jsonRaw = controller.Get(x);
                IChemical jsonResult = JsonConvert.DeserializeObject<Chemical>(jsonRaw);
                Assert.Equal(x, jsonResult.GetName());
                Assert.True(true == jsonResult.GetDeletedStatus());
                ListToRemove.Remove(x);
            }
            Assert.True(ListToRemove.Count == 0);

        }

        [Fact]
        public void PutChemical()
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
                controller.Post(x);
                ListToRemove.Add(temp);
                controller.Put(x, temp);
            }

            foreach (var x in ListOfUpdatedStrings)
            {
                string jsonRaw = controller.Get(x);
                IChemical jsonResult = JsonConvert.DeserializeObject<Chemical>(jsonRaw);
                Assert.Equal(x, jsonResult.GetName());
                ListToRemove.Remove(x);
            }

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
