using Custodial.Addressing.Service.Converters;
using Custodial.Addressing.Service.Interfaces;
using Custodial.Addressing.Service.Service_Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Addressing.Service
{
    public class Microservice : IDatabaseObject
    {
        [JsonIgnore]
        public IDatabase database { get; set; }
        public DateTime timeCreated { get; set; }
        public string iD { get; set; }
        public bool isDeleted { get; set; }
        public string serviceName { get; set; }
        [JsonConverter(typeof(ConcreteTypeConverter<ServiceSettings>))]
        public IServiceSettings settings { get; set; }
        public string discription { get; set; }
        public string shortName { get; set; }

        public async Task<IDatabaseObject> DeleteAsync(IDatabase database = null)
        {
            if (this.database == null)
            {
                this.database = database;
            }
            return await this.database.DeleteAsync(copyItself());
        }

        public async Task<IDatabaseObject> UpdateAsync(IDatabaseObject databaseObjectUpdated, IDatabase database = null)
        {
            if (this.database == null)
            {
                this.database = database;
            }
            return await this.database.UpdateAsync(copyItself(), databaseObjectUpdated);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(new Microservice()
            {
                timeCreated = timeCreated,
                iD = iD,
                isDeleted = isDeleted,
                serviceName = serviceName,
                settings = settings,
                discription = discription,
                shortName = shortName
            });
        }

        private IDatabaseObject copyItself()
        {
            return new Microservice()
            {
                timeCreated = timeCreated,
                iD = iD,
                isDeleted = isDeleted,
                serviceName = serviceName,
                settings = settings,
                discription = discription,
                shortName = shortName,
                database = null
            };
        }

        public static IDatabaseObject nullMicroService()
        {
            return new Microservice()
            {
                iD = null,
                serviceName = null,
                settings = null,
                discription = null,
                shortName = null,
                database = null
            };
        }
    }
}
