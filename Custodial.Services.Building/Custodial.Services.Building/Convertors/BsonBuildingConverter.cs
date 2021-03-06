﻿using Custodial.BoilerPlate.Core.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Services.Building.Convertors
{
    public class BsonBuildingConverter : IBsonConverter<Building>
    {
        public BsonDocument convertToBson(Building databaseObject)
        {
            return new BsonDocument
            {
                {"isDeleted", BsonValue.Create(databaseObject.isDeleted)},
                {"timeCreated", BsonValue.Create(databaseObject.timeCreated)},
                {"organizationId", BsonValue.Create(databaseObject.organizationId)},
                {"buildingName", BsonValue.Create(databaseObject.buildingName)},
                {"ammountOfFloors", BsonValue.Create(databaseObject.ammountOfFloors)},
                {"floors", BsonValue.Create(new BsonDocumentWrapper(databaseObject.floors))},
                {"buildingInventory", BsonValue.Create(new BsonDocumentWrapper(databaseObject.buildingInventory))},

            };
        }
    }
}
