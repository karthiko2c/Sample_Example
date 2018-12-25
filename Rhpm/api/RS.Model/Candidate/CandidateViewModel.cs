using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace RS.ViewModel.Candidate
{
    public class CandidateViewModel
    {
        public Guid CandidateId { get; set; }
        public int Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ExperienceYear { get; set; }
        public int ExperienceMonth { get; set; }
        public string Description { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int Qualification { get; set; }
        public string QualificationName { get; set; }
        public Guid OpeningId { get; set; }
        public string OpeningTitle { get; set; }
        public bool IsApproved { get; set; }
        public bool IsReadyForInterview { get; set; }
        public CandidateDocumentViewModel CandidateDocument { get; set; }
    }
}
