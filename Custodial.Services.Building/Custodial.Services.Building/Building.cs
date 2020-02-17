using Custodial.BoilerPlate.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Services.Building
{
    public class Building : IDatabaseObject
    {
        public IDatabase database { get; set; }
        public long timeCreated { get; set; }
        public string iD { get; set; }
        public bool isDeleted { get; set; }
        public string organizationId { get; set; }

        public async Task<IDatabaseObject> DeleteAsync(IDatabase database = null)
        {
            if (this.database == null)
            {
                this.database = database;
            }
            return await this.database.DeleteAsync(iD);
        }

        public IDatabaseObject NullObject()
        {
            return new Building()
            {
            };
        }

        public static IDatabaseObject NulledObject()
        {
            return new Building();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(copyIteself());
        }

        public async Task<IDatabaseObject> UpdateAsync(IDatabaseObject databaseObjectUpdated, IDatabase database = null)
        {
            if (this.database == null)
            {
                this.database = database;
            }
            return await this.database.UpdateAsync(iD, databaseObjectUpdated);

        }

        private IDatabaseObject copyIteself()
        {
            return new Building()
            {
                iD = iD,
                isDeleted = isDeleted,
                timeCreated = timeCreated,
                organizationId = organizationId
            };
        }
    }
}
