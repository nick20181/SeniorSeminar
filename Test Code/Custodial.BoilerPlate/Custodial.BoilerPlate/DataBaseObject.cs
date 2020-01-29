using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.BoilerPlate
{
    public class DataBaseObject : IDatabaseObject
    {
        public IDatabase database { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long timeCreated { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string iD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool isDeleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task<IDatabaseObject> DeleteAsync(IDatabase database = null)
        {
            throw new NotImplementedException();
        }

        public IDatabaseObject NullObject()
        {
            throw new NotImplementedException();
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public Task<IDatabaseObject> UpdateAsync(IDatabaseObject databaseObjectUpdated, IDatabase database = null)
        {
            throw new NotImplementedException();
        }
    }
}
