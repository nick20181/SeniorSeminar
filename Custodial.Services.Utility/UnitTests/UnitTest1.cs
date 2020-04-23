using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void test1()
        {
            string countries = System.IO.File.ReadAllText(@"C:\Users\Nicholas.bohm\Documents\countriesBefore.txt");
            string toPrint = "";
            string[] countiesList = Regex.Split(countries, @"\d{1,3}\t");
            foreach (var s in Regex.Split(countries, @"\t\w{1,11} {0,1}\w{0,11}ô{0,1}\w{0,11} {0,1}\w{0,11} {0,1}\w{0,11} {0,1}\w{0,11}\({0,1}\w{0,11}\-{0,1} {0,1}\w{0,11}\){0,1}"))
            {
                if (string.IsNullOrEmpty(s))
                {
                    break;
                }
                string p = countiesList[int.Parse(s.Replace("\r\n", ""))].Replace("\r", "").Replace("\n", "");
                toPrint = toPrint + countiesList[int.Parse(s.Replace("\r\n", ""))].Replace("\r", "").Replace("\n", "").Replace(" ", "_") + ",\n";
                

            }
            System.IO.File.WriteAllText(@"C:\Users\Nicholas.bohm\Documents\countires.txt", toPrint);
            Assert.IsTrue(true);
        }
    }
}
