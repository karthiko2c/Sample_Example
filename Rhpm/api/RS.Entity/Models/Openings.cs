using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS.Entity.Models
{
    public partial class Openings : BaseEntity
    {
        public Openings()
        {
            OpeningSkills = new HashSet<OpeningSkills>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OpeningId { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        public ICollection<OpeningSkills> OpeningSkills { get; set; }
    }
}
