using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SubQuip.Common.Enums;
using SubQuip.Entity.Models.BillOfMaterials;
using System;
using System.Collections.Generic;

namespace SubQuip.Entity.Models
{
    public class Request : BaseEntity
    {
        [BsonId]
        public ObjectId RequestId { get; set; }

        [BsonElement("type")]
        public RequestFormType Type { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("fromdate")]
        public DateTime FromDate { get; set; }

        [BsonElement("todate")]
        public DateTime ToDate { get; set; }

        [BsonElement("requested_by")]
        public string RequestedBy { get; set; }

        [BsonElement("company")]
        public string Company { get; set; }

        [BsonElement("phonenumber")]
        public string PhoneNumber { get; set; }

        [BsonElement("mailUsers")]
        public List<MailUserDetails> MailUsers { get; set; }

        [BsonElement("files")]
        public List<ObjectId> Files { get; set; }

        [BsonElement("regardingId")]
        public ObjectId? RegardingId { get; set; }

        [BsonElement("bomRequestNode")]
        public List<BomRequestNode> BomRequestNodes { get; set; }

    }
}
