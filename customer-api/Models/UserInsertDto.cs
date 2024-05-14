using System;
using MongoDB.Bson.Serialization.Attributes;

namespace customer_api.Models
{
	public class UserInsertDto
	{
        [BsonElement("Username")]
        public string Username { get; set; } = null!;

        [BsonElement("Password")]
        public string Password { get; set; } = null!;
    }
}

