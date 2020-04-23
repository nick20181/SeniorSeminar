using Custodial.Service.Organization.Service_Settings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Custodial.Service.Organization.Interfaces
{
    public interface IServiceController<databaseObjectType> where databaseObjectType : IDatabaseObject
    {
        public IDatabase database { get; set; }
        public IDatabaseObjectFactory factory { get; set; }
        public IServiceSettings settings { get; set; }

        Task<string> GetAsync([FromBody] databaseObjectType databaseObejct = default);
        Task<string> PostAsync([FromBody] databaseObjectType databaseObject);
        Task<string> DeleteAsync([FromBody] databaseObjectType databaseObject);
        Task<string> PutAsync([FromBody] List<databaseObjectType> databaseObjectList);

    }
}
