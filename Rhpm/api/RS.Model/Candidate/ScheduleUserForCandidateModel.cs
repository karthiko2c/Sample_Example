using RS.ViewModel.Approval;
using RS.ViewModel.User;
using System;
using System.Collections.Generic;

namespace RS.ViewModel.Candidate
{
    public class ScheduleUserForCandidateModel
    {
        public int ScheduleUserForCandidateId { get; set; }
        public int ApprovalEventId { get; set; }
        public Guid CandidateId { get; set; }
        public Guid UserId { get; set; }
        public DateTime ScheduledOn { get; set; }
        public string InterviewScheduledDate { get; set; }
        public string InterviewScheduledTime { get; set; }
        public bool IsFinished { get; set; }
        public UserViewModel User { get; set; }
        public ApprovalEventViewModel ApprovalEvent { get; set; }
        public CandidateViewModel Candidate { get; set; }
    }
}
