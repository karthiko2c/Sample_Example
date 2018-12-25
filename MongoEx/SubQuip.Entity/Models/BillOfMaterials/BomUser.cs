using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.Entity.Models.BillOfMaterials
{
    public class BomUser
    {
        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("mail")]
        public string Mail { get; set; }

        [BsonElement("company")]
        public string Company { get; set; }
    }
}
