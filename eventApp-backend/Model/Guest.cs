﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace eventApp_backend.Model
{
    public class Guest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

    }
}
