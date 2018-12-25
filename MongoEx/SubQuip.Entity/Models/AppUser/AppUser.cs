using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.Entity.Models.AppUser
{
    public class AppUser
    {
        [BsonId]
        public ObjectId UserId { get; set; }

        [BsonElement("mail")]
        public string Mail { get; set; }

        [BsonElement("savedTabs")]
        public List<SavedTab> SavedTabs { get; set; }
    }
}
