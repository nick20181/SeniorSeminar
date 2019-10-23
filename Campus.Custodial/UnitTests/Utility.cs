using Campus.Custodial.Chemicals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Campus.Custodial.Chemicals.Chemical;

namespace UnitTests
{
    public class Utility
    {
        public  Chemical createChemical(string name, int sigWords, IDatabase db, Manufacturer manu = null)
        {
            if (manu == null)
            {
                return new Chemical()
                {
                    name = name,
                    DB = db,
                    manufacturer = createManufactuerer(RandomString(10), RandomString(10), RandomString(5)),
                    hazardStatements = generateStringList(3),
                    precautionStatements = generateStringList(2),
                    sigWords = generateSigWords(sigWords)
                };
            }
            return new Chemical()
            {
                name = name,
                DB = db,
                manufacturer = manu,
                hazardStatements = generateStringList(3),
                precautionStatements = generateStringList(2),
                sigWords = generateSigWords(sigWords)
            };
        }
        public List<signalWords> generateSigWords(int count = 0)
        {
            List<signalWords> toReturn = new List<signalWords>();
            for (int i = 0; i < count; i++)
            {
                toReturn.Add((signalWords)random.Next(0, 1));
            }
            return toReturn;
        }
        public List<string> generateStringList(int size)
        {
            List<string> toReturn = new List<string>();
            for (int i = 0; i < size; i++)
            {
                toReturn.Add(RandomString(10));
            }
            return toReturn;
        }
        public Manufacturer createManufactuerer(string name, string address, string phonenum)
        {
            return new Manufacturer()
            {
                name = name,
                address = address,
                phoneNumber = phonenum
            };
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
