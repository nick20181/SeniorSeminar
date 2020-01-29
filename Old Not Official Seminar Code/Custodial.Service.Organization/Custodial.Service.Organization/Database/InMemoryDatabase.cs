using Custodial.Service.Organization.Service_Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Service.Organization.Database
{
    public class InMemoryDatabase<databaseObjectType> : IDatabase where databaseObjectType : IDatabaseObject
    {
        public IDatabaseSettings settings { get; set; }
        private Dictionary<string, IDatabaseObject> database = new Dictionary<string, IDatabaseObject>();

        public InMemoryDatabase(IDatabaseSettings settings)
        {
            this.settings = settings;
        }

        public Task<IDatabaseObject> CreateAsync(IDatabaseObject databaseObject)
        {
            IDatabaseObject toReturn;
            databaseObjectType microservice = (databaseObjectType)databaseObject;
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
                        databaseObjectType databaseObjectToAdd = (databaseObjectType)databaseObject;
                        databaseObjectToAdd.isDeleted = true;
                        database.Remove(databaseObject.iD);
                        database.Add(databaseObject.iD, databaseObjectToAdd);
                        return databaseObjectToAdd;
                    }
                }
            }
            return null;
        }

        public async Task<List<IDatabaseObject>> ReadAsync(IDatabaseObject databaseObject = null)
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

        public async Task<IDatabaseObject> UpdateAsync(IDatabaseObject databaseObjectOrginal, IDatabaseObject databaseObjectUpdated)
        {
            databaseObjectType temp;
            if (databaseObjectOrginal != null || databaseObjectUpdated != null)
            {
                foreach (var dataObject in await ReadAsync(databaseObjectOrginal))
                {
                    if (dataObject.iD.Equals(databaseObjectOrginal.iD))
                    {
                        temp = (databaseObjectType)dataObject;
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
