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
        public async Task<IDatabaseObject> DeleteAsync(string dataObjectId)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (!string.IsNullOrEmpty(dataObjectId))
            {
                foreach (var ms in await ReadAsync($"_id", dataObjectId))
                {
                    if (ms.iD.Equals(dataObjectId))
                    {
                        databaseObject databaseObjectToAdd = (databaseObject) ms;
                        databaseObjectToAdd.isDeleted = true;
                        database.Remove(dataObjectId);
                        database.Add(dataObjectId, databaseObjectToAdd);
                        return databaseObjectToAdd;
                    }
                }
            }
            return null;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<IDatabaseObject>> ReadAsync(string stringFilter = null, string data = null)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            List<IDatabaseObject> toReturn = new List<IDatabaseObject>();
            if (string.IsNullOrEmpty(stringFilter))
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
                    if (dataObject.Key.Equals(data))
                    {
                        toReturn.Add(dataObject.Value);
                        return toReturn;
                    }
                }
            }
            return null;
        }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IDatabaseObject> UpdateAsync(string databaseObjectOrginalId, IDatabaseObject databaseObjectUpdated)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (String.IsNullOrEmpty(databaseObjectOrginalId) || databaseObjectUpdated != null)
            {
                foreach (var dataObject in await ReadAsync("_id", databaseObjectOrginalId))
                {
                    if (dataObject.iD.Equals(databaseObjectOrginalId))
                    {
                        database.Remove(databaseObjectOrginalId);
                        databaseObjectUpdated.iD = databaseObjectOrginalId;
                        database.Add(databaseObjectOrginalId, databaseObjectUpdated);
                        return databaseObjectUpdated;
                    }
                }
            }
            return null;


        }
    }
}
