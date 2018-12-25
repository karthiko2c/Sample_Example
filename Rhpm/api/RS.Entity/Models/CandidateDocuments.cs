using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS.Entity.Models
{
    public partial class CandidateDocuments : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CandidateDocumentId { get; set; }

        public Guid CandidateId { get; set; }

        [Required, MaxLength(150)]
        public string DocumentName { get; set; }

        [Required, MaxLength(150)]
        public string UploadedDocument { get; set; }

        public Candidates Candidate { get; set; }

    }
}
