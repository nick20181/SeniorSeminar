using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Custodial.BoilerPlate.Core.Interfaces
{
    public interface IBsonConverter<DatabaseObject> where DatabaseObject : IDatabaseObject
    {
        public BsonDocument convertToBson(DatabaseObject databaseObject);
    }
}
