using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS.Entity.Models
{
    public partial class ApprovalEventRoles : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ApprovalEventRoleId { get; set; }
        public int ApprovalEventId { get; set; }
        public int RoleId { get; set; }

        public ApprovalEvents ApprovalEvent { get; set; }
        public Roles Role { get; set; }

        public Guid UserId { get; set; }
        public Users User { get; set; }
    }
}
