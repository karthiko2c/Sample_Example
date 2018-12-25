using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS.Entity.Models
{
    public partial class ApprovalActions
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ApprovalActionId { get; set; }

        [Required, MaxLength(150)]
        public string ApprovalActionName { get; set; }

        public int ApprovalEventId { get; set; }

        public ApprovalEvents ApprovalEvent { get; set; }

    }
}
