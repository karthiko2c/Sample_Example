using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using SubQuip.Common.Enums;

namespace SubQuip.Entity.Models.BillOfMaterials
{
    /// <summary>
    /// Specifies the single comment added to the BOM
    /// </summary>
    public class BomComment: BaseEntity
    {
        /// <summary>
        /// comment
        /// </summary>
        [BsonElement("comment")]
        public string Comment { get; set; }

        /// <summary>
        /// who added the comment
        /// </summary>
        [BsonElement("created_by_name")]
        public string CreatedByName { get; set; }

    }
}
