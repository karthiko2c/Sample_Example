using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.Entity.Models
{
    public class Location
    {
        [BsonId]
        public ObjectId LocationId { get; set; }

        [BsonElement("locationName")]
        public string LocationName { get; set; }
    }
}
