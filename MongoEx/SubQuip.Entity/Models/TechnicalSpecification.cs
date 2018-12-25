using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SubQuip.Entity.Models
{
	public class TechnicalSpecification : BaseEntity
	{
		[BsonId]
		public ObjectId TechSpecId { get; set; }

		[BsonElement("techSpecName")]
		public string TechSpecName { get; set; }

		[BsonElement("value")]
		public string Value { get; set; }
        
		[BsonElement("includeInOverview")]
		public bool IncludeInOverview { get; set; }

    }
}