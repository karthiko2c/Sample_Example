using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SubQuip.Entity.Models
{
    public class Document
    {
        [BsonId]
        public ObjectId DocumentId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

		[BsonElement("file")]
		public ObjectId File { get; set; }

    }
}
