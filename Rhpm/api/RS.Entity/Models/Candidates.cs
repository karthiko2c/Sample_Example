using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS.Entity.Models
{
    public partial class Candidates : BaseEntity
    {
        public Candidates()
        {
            CandidateDocuments = new HashSet<CandidateDocuments>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CandidateId { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public int ExperienceYear { get; set; }

        [Required]
        public int ExperienceMonth { get; set; }

        [Required]
        public int QualificationId { get; set; }

        [Required]
        public int OrganizationId { get; set; }

        public Organizations Organisation { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsApproved { get; set; }

        public bool IsReadyForInterview { get; set; }

        public Qualifications Qualification { get; set; }

        public ICollection<CandidateDocuments> CandidateDocuments { get; set; }
    }
}
