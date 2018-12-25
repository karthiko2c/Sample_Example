using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RS.Entity.Models
{
    public partial class ScheduleUserForCandidate : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ScheduleUserForCandidateId { get; set; }
        public int ApprovalEventId { get; set; }
        public Guid CandidateId { get; set; }
        public Guid UserId { get; set; }
        public bool IsFinished { get; set; }
        public bool IsStarted { get; set; }
        public ApprovalEvents ApprovalEvent { get; set; }
        public Candidates Candidate { get; set; }
        public DateTime ScheduledOn { get; set; }
        public Users User { get; set; }
    }
}
