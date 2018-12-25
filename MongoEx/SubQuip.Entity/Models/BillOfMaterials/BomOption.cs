using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SubQuip.Entity.Models.BillOfMaterials
{
    public class BomOption
    {
        [BsonElement("license")]
        public ObjectId? License { get; set; }

        [BsonElement("owner")]
        public ObjectId? Owner { get; set; }

        [BsonElement("location")]
        public ObjectId? Location { get; set; }
    }
}
