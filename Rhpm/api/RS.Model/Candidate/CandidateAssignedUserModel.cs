using RS.ViewModel.Approval;
using RS.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace RS.ViewModel.Candidate
{
    public class CandidateAssignedUserModel
    {
        public int CandidateAssignedUserId { get; set; }
        public int ApprovalEventId { get; set; }
        public Guid CandidateId { get; set; }

        public ApprovalEventViewModel ApprovalEvent { get; set; }
        public CandidateViewModel Candidate { get; set; }

        public Guid UserId { get; set; }
        public UserViewModel User { get; set; }
    }
}
