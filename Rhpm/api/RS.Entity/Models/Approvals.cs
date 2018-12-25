using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS.Entity.Models
{
    public partial class Approvals
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ApprovalId { get; set; }

        [Required, MaxLength(150)]
        public string ApprovalName { get; set; }

        [MaxLength(500)]
        public string ApprovalDesc { get; set; }

    }
}
