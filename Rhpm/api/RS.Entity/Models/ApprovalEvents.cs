using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS.Entity.Models
{
    public partial class ApprovalEvents
    {
        public ApprovalEvents()
        {
            ApprovalActions = new HashSet<ApprovalActions>();
            ApprovalEventRoles = new HashSet<ApprovalEventRoles>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ApprovalEventId { get; set; }

        [Required, MaxLength(150)]
        public string ApprovalEventName { get; set; }

        public int ApprovalEventOrder { get; set; }

        public int ApprovalId { get; set; }

        public Approvals Approval { get; set; }

        public ICollection<ApprovalActions> ApprovalActions { get; set; }

        public ICollection<ApprovalEventRoles> ApprovalEventRoles { get; set; }
    }
}
