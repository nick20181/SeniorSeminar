using Custodial.Addressing.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Addressing.Service.Databases
{
    public class InMemoryDatabase : IDatabase
    {
        public IDatabaseSettings settings { get; set; }
        private Dictionary<string, IDatabaseObject> database = new Dictionary<string, IDatabaseObject>();

        public Task<IDatabaseObject> CreateAsync(IDatabaseObject databaseObject)
        {
            IDatabaseObject toReturn;
            Microservice microservice = (Microservice)databaseObject;
            database.Add(microservice.iD, databaseObject);
            database.TryGetValue(databaseObject.iD, out toReturn);
            return Task.FromResult(toReturn);
        }

        public async Task<IDatabaseObject> DeleteAsync(IDatabaseObject databaseObject)
        {
            if (!databaseObject.Equals(null))
            {
                foreach (var ms in await ReadAsync(databaseObject))
                {
                    if (ms.iD.Equals(databaseObject.iD))
                    {
                        Microservice msToAdd = (Microservice)databaseObject;
                        msToAdd.isDeleted = true;
                        database.Remove(databaseObject.iD);
                        database.Add(databaseObject.iD, msToAdd);
                        return msToAdd;
                    }
                }
            }
            return new Microservice();
        }

        public async Task<List<IDatabaseObject>> ReadAsync(IDatabaseObject databaseObject = null)
        {
            List<IDatabaseObject> toReturn = new List<IDatabaseObject>();
            if (databaseObject == null)
            {
                foreach (var ms in database)
                {
                    toReturn.Add(ms.Value);
                }
                return toReturn;
            }
            else
            {
                foreach (var ms in database)
                {
                    if (ms.Key.Equals(databaseObject.iD))
                    {
                        toReturn.Add(ms.Value);
                        return toReturn;
                    }
                }
            }
            return new List<IDatabaseObject>();
        }

        public async Task<IDatabaseObject> UpdateAsync(IDatabaseObject databaseObjectOrginal, IDatabaseObject databaseObjectUpdated)
        {
            Microservice temp;
            if (databaseObjectOrginal != null || databaseObjectUpdated != null)
            {
                foreach (var ms in await ReadAsync(databaseObjectOrginal))
                {
                    if (ms.iD.Equals(databaseObjectOrginal.iD))
                    {
                        temp = (Microservice)ms;
                        database.Remove(ms.iD);
                        databaseObjectUpdated.iD = temp.iD;
                        database.Add(temp.iD, databaseObjectUpdated);
                        return databaseObjectUpdated;
                    }
                }
            }
            return new Microservice();


        }
    }
}
