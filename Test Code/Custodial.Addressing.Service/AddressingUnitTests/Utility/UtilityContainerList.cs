using Custodial.Addressing.Service;
using Custodial.Addressing.Service.Interfaces;
using Custodial.Addressing.Service.Service_Settings;
using Custodial.Addressing.Service.Service_Settings.Utility;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AddressingUnitTests.Utility
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
                var ms = new Microservice()
                {
                    discription = RandomString(10),
                    database = null,
                    isDeleted = false,
                    serviceName = RandomString(10),
                    settings = new ServiceSettings()
                    {
                        databaseSettings = new DatabaseSettings()
                        {
                            address = RandomString(10),
                            port = RandomString(10),
                            databaseItems = new List<DatabaseCollection>()
                            {
                                new DatabaseCollection()
                                {
                                    databaseName = RandomString(10),
                                    collectionNames = new List<string>()
                                    {
                                        RandomString(10)
                                    }
                                }
                            }
                        },
                        networkSettings = new NetworkSettings()
                        {
                            addresses = new List<string>()
                            {
                                RandomString(10), RandomString(10)
                            },
                            ports = new List<string>()
                            {
                                RandomString(10)
                            }
                        }
                    },
                    shortName = RandomString(3),
                    timeCreated = DateTime.UtcNow.AddDays(rando.NextDouble()).Ticks
                };
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
