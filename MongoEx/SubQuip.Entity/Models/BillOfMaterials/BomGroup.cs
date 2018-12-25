using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using SubQuip.Common.Enums;

namespace SubQuip.Entity.Models.BillOfMaterials
{
    /// <summary>
    /// Group either contains BOM or Equipment/Material at a particular instance.
    /// </summary>
    public class BomGroup
    {
        /// <summary>
        /// Group Name
        /// </summary>
        [BsonElement("groupName")]
        public string Name { get; set; }

        /// <summary>
        /// Bill of material
        /// </summary>
        [BsonElement("billOfMaterials")]
        public IEnumerable<BillOfMaterial> BillOfMaterials { get; set; }

        /// <summary>
        /// Items can be equipment or material
        /// </summary>
        [BsonElement("bomItems")]
        public IEnumerable<BomItem> BomItems { get; set; }

    }
}
