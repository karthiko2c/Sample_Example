using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SubQuip.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.Entity.Models.BillOfMaterials
{
    public class AddToBom
    {
        [BsonElement("regardingId")]
        public ObjectId RegardingId { get; set; }

        [BsonElement("itemType")]
        public BomItemType ItemType { get; set; }
    }
}
