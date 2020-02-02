using Custodial.BoilerPlate.Core;
using Custodial.BoilerPlate.Core.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.AddressingServices
{
    public class BsonMicroserviceConverter : IBsonConverter<Microservice>
    {
        public BsonDocument convertToBson(Microservice databaseObject)
        {
            return new BsonDocument
            {
                {"timeCreated", BsonValue.Create(databaseObject.timeCreated)},
                {"isDeleted", BsonValue.Create(databaseObject.isDeleted)},
                {"serviceName", BsonValue.Create(databaseObject.serviceName)},
                {"shortName", BsonValue.Create(databaseObject.shortName)},
                {"discription", BsonValue.Create(databaseObject.discription)},
                {"settings", BsonValue.Create(new BsonDocumentWrapper(databaseObject.settings))}
            };
        }
    }
}
