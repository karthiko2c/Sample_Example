using System;
using System.Collections.Generic;
using System.Text;

namespace RS.ViewModel.Candidate
{
    public class CandidateListModel
    {
        public Guid CandidateId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ExperienceYear { get; set; }
        public int ExperienceMonth { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public String Opening { get; set; }
        public bool IsApproved { get; set; }
        public bool IsFinished { get; set; }
        public bool IsReadyForInterview { get; set; }
        public int ApprovalEventId { get; set; }
        public int AssignedUsers { get; set; }
        public string Status { get; set; }
        public int Documents { get; set; }
    }
}
