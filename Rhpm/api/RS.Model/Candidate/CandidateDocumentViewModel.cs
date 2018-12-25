using System;

namespace RS.ViewModel.Candidate
{
    public class CandidateDocumentViewModel
    {
        public Guid CandidateDocumentId { get; set; }

        public Guid CandidateId { get; set; }

        public string DocumentName { get; set; }

        public string UploadedDocument { get; set; }
    }
}
