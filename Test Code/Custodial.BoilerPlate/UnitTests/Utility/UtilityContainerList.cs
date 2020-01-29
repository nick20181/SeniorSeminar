﻿using Custodial.BoilerPlate;
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
                //Add custom type here!
                IDatabaseObject ms = null;
                bool hasId = false;
                ms.iD = IDGen.GetId(ms, out hasId).ToString();
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