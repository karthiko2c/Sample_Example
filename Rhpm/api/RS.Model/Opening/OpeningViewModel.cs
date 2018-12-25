using RS.ViewModel.Approval;
using RS.ViewModel.Candidate;
using RS.ViewModel.Skill;
using System;
using System.Collections.Generic;
using System.Text;

namespace RS.ViewModel.Opening
{
    public class OpeningViewModel
    {
        public Guid OpeningId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsApproved { get; set; }
        public List<SkillViewModel> PrimarySkillTypes { get; set; }
        public List<SkillViewModel> SecondarySkillTypes { get; set; }
        public string PrimarySkills { get; set; }
        public string SecondarySkills { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Status { get; set; }
    }

    public class EntityAndApprovalViewModel
    {
        public CandidateViewModel candidateViewModel { get; set; }
        public OpeningViewModel openingViewModel { get; set; }
        public ApprovalTransactionViewModel approvalTransactionViewModel { get; set; }
    }

    public class ApprovalEventAndTransactionDetail
    {
        public List<ApprovalEventViewModel> approvalEventViewModel { get; set; }
        public ApprovalTransactionViewModel approvalTransactionViewModel { get; set; }
    }
}
