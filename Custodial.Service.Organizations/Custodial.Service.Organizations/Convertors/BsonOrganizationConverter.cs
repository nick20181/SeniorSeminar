using Custodial.BoilerPlate.Core.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Service.Organizations.Convertors
{
    public class BsonOrganizationConverter : IBsonConverter<Organization>
    {
        public BsonDocument convertToBson(Organization databaseObject)
        {
            return new BsonDocument
            {
                {"activeService", BsonValue.Create(databaseObject.activeService) },
                {"organizationAddress", BsonValue.Create(databaseObject.organizationAddress)},
                {"isDeleted", BsonValue.Create(databaseObject.isDeleted)},
                {"organizationName", BsonValue.Create(databaseObject.organizationName) },
                {"phoneNumber", BsonValue.Create(databaseObject.phoneNumber)},
                {"timeCreated", BsonValue.Create(databaseObject.timeCreated)}
            };
        }
    }
}
