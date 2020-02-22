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
                {"chemicalManufactor", BsonValue.Create(new BsonDocumentWrapper(databaseObject.chemicalManufactor))},
                {"saftyContactInformation", BsonValue.Create(new BsonDocumentWrapper(databaseObject.saftyContactInformation))},
                {"chemicalWarning", BsonValue.Create(databaseObject.chemicalWarning)},
                {"disinfectant", BsonValue.Create(databaseObject.disinfectant)},
                {"ventilationNeeded", BsonValue.Create(databaseObject.ventilationNeeded)},
                {"usesAndPrep", BsonValue.Create(new BsonDocumentWrapper(databaseObject.usesAndPrep))}
            };
        }
    }
}
