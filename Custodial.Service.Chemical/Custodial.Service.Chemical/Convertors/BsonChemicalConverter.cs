using Custodial.BoilerPlate.Core.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Service.Chemical.Convertors
{
    public class BsonChemicalConverter : IBsonConverter<Chemical>
    {
        public BsonDocument convertToBson(Chemical databaseObject)
        {
            return new BsonDocument
            {
                {"isDeleted", BsonValue.Create(databaseObject.isDeleted)},
                {"timeCreated", BsonValue.Create(databaseObject.timeCreated)},
                {"organizationId", BsonValue.Create(databaseObject.organizationId)},
                {"chemicalName", BsonValue.Create(databaseObject.chemicalName)},
                {"chemcialIngredients", BsonValue.Create((new BsonDocumentWrapper(databaseObject.chemcialIngredients)))},
                {"chemicalStoringInformation", BsonValue.Create(databaseObject.chemicalStoringInformation)},
                {"ventilationNeeded", BsonValue.Create(databaseObject.ventilationNeeded)}
            };
        }
    }
}
