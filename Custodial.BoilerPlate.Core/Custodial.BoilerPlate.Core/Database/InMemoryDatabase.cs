using Custodial.BoilerPlate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Core.Database
{
    public class InMemoryDatabase<databaseObject> : IDatabase where databaseObject : IDatabaseObject
    {
        public IDatabaseSettings settings { get; set; }
        private Dictionary<string, IDatabaseObject> database = new Dictionary<string, IDatabaseObject>();

        public InMemoryDatabase(IDatabaseSettings settings)
        {
            this.settings = settings;
        }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        public Task<IDatabaseObject> CreateAsync(IDatabaseObject databaseObject)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            IDatabaseObject toReturn;
            databaseObject microservice = (databaseObject)databaseObject;
            database.Add(microservice.iD, databaseObject);
            database.TryGetValue(databaseObject.iD, out toReturn);
            return Task.FromResult(toReturn);
        }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IDatabaseObject> DeleteAsync(IDatabaseObject databaseObject)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (!databaseObject.Equals(null))
            {
                foreach (var ms in await ReadAsync(databaseObject))
                {
                    if (ms.iD.Equals(databaseObject.iD))
                    {
                        databaseObject databaseObjectToAdd = (databaseObject)databaseObject;
                        databaseObjectToAdd.isDeleted = true;
                        database.Remove(databaseObject.iD);
                        database.Add(databaseObject.iD, databaseObjectToAdd);
                        return databaseObjectToAdd;
                    }
                }
            }
            return null;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<IDatabaseObject>> ReadAsync(IDatabaseObject databaseObject = null)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            List<IDatabaseObject> toReturn = new List<IDatabaseObject>();
            if (databaseObject == null)
            {
                foreach (var dataObject in database)
                {
                    toReturn.Add(dataObject.Value);
                }
                return toReturn;
            }
            else
            {
                foreach (var dataObject in database)
                {
                    if (dataObject.Key.Equals(databaseObject.iD))
                    {
                        toReturn.Add(dataObject.Value);
                        return toReturn;
                    }
                }
            }
            return null;
        }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IDatabaseObject> UpdateAsync(IDatabaseObject databaseObjectOrginal, IDatabaseObject databaseObjectUpdated)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            databaseObject temp;
            if (databaseObjectOrginal != null || databaseObjectUpdated != null)
            {
                foreach (var dataObject in await ReadAsync(databaseObjectOrginal))
                {
                    if (dataObject.iD.Equals(databaseObjectOrginal.iD))
                    {
                        temp = (databaseObject)dataObject;
                        database.Remove(dataObject.iD);
                        databaseObjectUpdated.iD = temp.iD;
                        database.Add(temp.iD, databaseObjectUpdated);
                        return databaseObjectUpdated;
                    }
                }
            }
            return null;


        }
    }
}
