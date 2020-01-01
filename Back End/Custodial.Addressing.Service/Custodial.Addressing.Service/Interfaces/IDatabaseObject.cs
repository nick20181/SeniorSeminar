﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custodial.Addressing.Service.Interfaces
{
    public interface IDatabaseObject
    {
        long timeCreated { get; set; }
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        string iD { get; set; }
        bool isDeleted { get; set; }
        string ToJson();
        Task<IDatabaseObject> UpdateAsync(IDatabaseObject databaseObjectUpdated, IDatabase database = null);
        Task<IDatabaseObject> DeleteAsync(IDatabase database = null);
    }
}
