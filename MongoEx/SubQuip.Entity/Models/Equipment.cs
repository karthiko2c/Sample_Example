using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SubQuip.Entity.Models.BillOfMaterials;
using System.Collections.Generic;

namespace SubQuip.Entity.Models
{
    [BsonDiscriminator("Equipment")]
    public class Equipment : BillOfMaterialsItem
    {
        [BsonId]
        public ObjectId EquipmentId { get; set; }

        [BsonElement("equipment_number")]
        public string EquipmentNumber { get; set; }

        [BsonElement("serial_number")]
        public string SerialNumber { get; set; }

        [BsonElement("owner")]
        public string Owner { get; set; }

        [BsonElement("license")]
        public string License { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("material")]
        public ObjectId Material { get; set; }

        [BsonElement("partProperties")]
        public List<PartProperty> PartProperties { get; set; }

        [BsonElement("documents")]
        public List<ObjectId> Documents { get; set; }

        [BsonElement("technicalSpecifications")]
        public List<TechnicalSpecification> TechnicalSpecifications { get; set; }
    }
}
