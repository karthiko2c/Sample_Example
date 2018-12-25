using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RS.Entity.Models
{
    public partial class CandidateAssignedUser: BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int CandidateAssignedUserId { get; set; }
        public int ApprovalEventId { get; set; }
        public Guid CandidateId { get; set; }

        public ApprovalEvents ApprovalEvent { get; set; }
        public Candidates Candidate { get; set; }

        public Guid UserId { get; set; }
        public Users User { get; set; }
    }
}
