using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.Entity.Models.AppUser
{
    public class SavedTab : BaseEntity
    {
        [BsonId]
        public ObjectId TabId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("path")]
        public string Path { get; set; }
    }
}
