using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SubQuip.Entity.Models.BillOfMaterials;

namespace SubQuip.Entity.Models
{
    [BsonDiscriminator("Material")]
    public class Material : BillOfMaterialsItem
    {
        [BsonId]
        public ObjectId MaterialId { get; set; } //Should be unique, hidden from user

        [BsonElement("materialnumber")]
        public string Materialnumber { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("manufactor_part_number")]
        public string ManufactorPartNumber { get; set; }

        [BsonElement("manufactor_name")]
        public string ManufactorName { get; set; }

        [BsonElement("owner")]
        public string Owner { get; set; }

        [BsonElement("license")]
        public string License { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("grossWeight")]
        public string GrossWeight { get; set; }

        [BsonElement("sizeDimension")]
        public string SizeDimension { get; set; }

        [BsonElement("quantity")]
        public string Quantity { get; set; }


        [BsonElement("IsSparePart")]
        public bool IsSparePart { get; set; }

        [BsonElement("partProperties")]
        public List<PartProperty> PartProperties { get; set; }

        [BsonElement("documents")]
        public List<ObjectId> Documents { get; set; }

        [BsonElement("technicalSpecifications")]
        public List<TechnicalSpecification> TechnicalSpecifications { get; set; }

    }
}
