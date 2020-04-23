using Custodial.BoilerPlate.Core.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    class BsonDatabaseObjectConverter : IBsonConverter<DataBaseObject>
    {
        public BsonDocument convertToBson(DataBaseObject databaseObject)
        {
            return new BsonDocument
            {   
                {"isDeleted", BsonValue.Create(databaseObject.isDeleted)},
                {"timeCreated", BsonValue.Create(databaseObject.timeCreated)}
            };
        }
    }
}
