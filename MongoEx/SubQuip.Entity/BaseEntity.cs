using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SubQuip.Entity
{
    public class BaseEntity
    {
        [BsonElement("isActive")]
        public bool IsActive { get; set; }

        [BsonElement("createdDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; }

        [BsonElement("modifiedDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ModifiedDate { get; set; }

        [BsonElement("createdBy")]
        public string CreatedBy { get; set; }

        [BsonElement("modifiedBy")]
        public string ModifiedBy { get; set; }

        // public bool IsDeleted { get; set; }
        //  public Guid? DeletedBy { get; set; }
        //  public DateTime? DeletedDate { get; set; }
    }
}
