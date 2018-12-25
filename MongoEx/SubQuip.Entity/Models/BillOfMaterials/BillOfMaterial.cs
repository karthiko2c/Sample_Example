using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SubQuip.Common.Enums;

namespace SubQuip.Entity.Models.BillOfMaterials
{
    public class BillOfMaterial : BaseEntity
    {
        [BsonId]
        public ObjectId BomId { get; set; }

        /// <summary>
        /// BomUser is the user who is set as owner. Initially, it will be the same as CreatedByUser. 
        /// But we will provision for ownership in the model
        /// </summary>
        [BsonElement("bomUser")]
        public BomUser BomUser { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("image")]
        public ObjectId? Image { get; set; }

        [BsonElement("status")]
        public BomStatus Status { get; set; }

        [BsonElement("type")]
        public BomType Type { get; set; }

        /// <summary>
        /// template id itself a bom id, when used as template for creating a new bom.
        /// </summary>
        [BsonElement("templateId")]
        public ObjectId? TemplateId { get; set; }

        [BsonElement("groups")]
        public List<BomGroup> Groups { get; set; }

        [BsonElement("addedItems")]
        public List<AddToBom> AddedItems { get; set; }

        [BsonElement("comments")]
        public List<BomComment> Comments { get; set; }

        [BsonElement("option")]
        public BomOption Option { get; set; }
    }
}
