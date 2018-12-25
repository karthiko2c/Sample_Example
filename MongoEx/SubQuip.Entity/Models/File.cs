using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SubQuip.Common.Enums;

namespace SubQuip.Entity.Models
{
    public class File: BaseEntity
    {
        [BsonId]
        public ObjectId FileId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

		[BsonElement("title")]
        public string Title { get; set; }

		[BsonElement("contentType")]
        public string ContentType { get; set; }

		[BsonElement("content")]
		public string Content { get; set; }

		[BsonElement("description")]
        public string Description { get; set; }
    }
}
