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
                {"organizationLocations", BsonValue.Create(new BsonDocumentWrapper(databaseObject.organizationLocations))},
                {"isDeleted", BsonValue.Create(databaseObject.isDeleted)},
                {"organizationName", BsonValue.Create(databaseObject.organizationName) },
                {"contactDetails", BsonValue.Create(new BsonDocumentWrapper(databaseObject.contactDetails))},
                {"timeCreated", BsonValue.Create(databaseObject.timeCreated)},
                {"employeeCount", BsonValue.Create(databaseObject.employeeCount)}
            };
        }
    }
}
