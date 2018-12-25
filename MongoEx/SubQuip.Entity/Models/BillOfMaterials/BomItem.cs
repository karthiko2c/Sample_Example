using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using SubQuip.Common.Enums;

namespace SubQuip.Entity.Models.BillOfMaterials
{
    /// <summary>
    /// Bom Items is either material/Equipment
    /// </summary>
    public class BomItem
    {
        /// <summary>
        /// Equipment or material type
        /// </summary>
        [BsonElement("itemType")]
        public BomItemType ItemType { get; set; }

        /// <summary>
        /// Equipment or material
        /// </summary>
        [BsonElement("item")]
        public BillOfMaterialsItem Item { get; set; }
    }
}
