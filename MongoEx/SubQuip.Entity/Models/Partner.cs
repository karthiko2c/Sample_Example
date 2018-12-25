using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.Entity.Models
{
    public class Partner: BaseEntity
    {
        [BsonId]
        public ObjectId PartnerId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

    }
}
