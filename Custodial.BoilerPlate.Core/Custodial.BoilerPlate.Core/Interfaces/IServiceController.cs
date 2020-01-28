using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate.Core.Interfaces
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
