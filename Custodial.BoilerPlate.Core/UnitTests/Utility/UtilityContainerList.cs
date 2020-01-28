using Custodial.BoilerPlate;
using Custodial.BoilerPlate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace UnitTests.Utility
{
    public class UtilityContainerList
    {
        public List<UtilityContainer> testList = new List<UtilityContainer>();
        private ObjectIDGenerator IDGen = new ObjectIDGenerator();
        public UtilityContainerList(int countOfItems)
        {
            for (int i = 0; i < countOfItems; i++)
            {
                Random rando = new Random();
                IDatabaseObject ms = new DataBaseObject();
                ms.timeCreated = rando.Next();
                testList.Add(new UtilityContainer(ms));
            }
        }

        public IDatabaseObject GetItem()
        {
            foreach (var x in testList)
            {
                if (!x.isUsed)
                {
                    x.isUsed = true;
                    return x.service;
                }
            }
            return null;
        }

        private string RandomString(int size, bool lowerCase = true)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}
