using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS.Entity.Models
{
    public partial class ApprovalTransactionDetails : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ApprovalTransactionDetailId { get; set; }
        public int ApprovalTransactionId { get; set; }
        public int EventOrderNumber { get; set; }
        public int ApprovalActionId { get; set; }

        [MaxLength(500)]
        public string Comments { get; set; }
       
    }
}
