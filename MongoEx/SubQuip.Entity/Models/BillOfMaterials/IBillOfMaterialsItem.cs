using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace SubQuip.Entity.Models.BillOfMaterials
{
    [BsonKnownTypes(typeof(Material), typeof(Equipment))]
    public class BillOfMaterialsItem: BaseEntity
    {
    }
}
