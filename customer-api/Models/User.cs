using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace customer_api.Models
{
	public class User
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; } = null!;

        [BsonElement("Password")]
        public string Password { get; set; } = null!;
    }
}

