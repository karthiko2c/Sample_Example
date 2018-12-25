using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS.Entity.Models
{
    public partial class ApprovalTransactions : BaseEntity
    {
        public ApprovalTransactions()
        {
            ApprovalTransactionDetails = new HashSet<ApprovalTransactionDetails>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ApprovalTransactionId { get; set; }
        public int ApprovalId { get; set; }
        public int EventOrderNumber { get; set; }
        public int NextEventOrderNumber { get; set; }
        public int ApprovalActionId { get; set; }
        public Guid EntityId { get; set; }
        public int EntityType { get; set; }
        public bool IsApproved { get; set; }
        public bool IsFurtherActionRequired { get; set; }

        public ApprovalActions ApprovalAction { get; set; }
        public ICollection<ApprovalTransactionDetails> ApprovalTransactionDetails { get; set; }
    }
}
