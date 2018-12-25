using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS.Entity.Models
{
    public partial class Qualifications :BaseEntity
    {
        public Qualifications()
        {
            Candidate = new HashSet<Candidates>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int QualificationId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        public ICollection<Candidates> Candidate { get; set; }
    }
}
