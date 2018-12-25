using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SubQuip.Entity.Models
{
   public class PartProperty
    {
        [BsonId]
        public ObjectId PartPropertyId { get; set; }

        [BsonElement("propertyName")]
        public string PropertyName { get; set; }

        [BsonElement("propertyValue")]
        public string PropertyValue { get; set; }
    }
}
