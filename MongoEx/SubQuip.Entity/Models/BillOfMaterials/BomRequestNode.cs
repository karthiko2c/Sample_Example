using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SubQuip.Common.Enums;

namespace SubQuip.Entity.Models.BillOfMaterials
{
    public class BomRequestNode
    {
        [BsonElement("regardingId")]
        public ObjectId RegardingId { get; set; }

        [BsonElement("type")]
        public BomItemType Type { get; set; }
    }
}
