using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS.Entity.Models
{
    public partial class Skills : BaseEntity
    {
        public Skills()
        {
            OpeningSkills = new HashSet<OpeningSkills>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int SkillId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
       
        public ICollection<OpeningSkills> OpeningSkills { get; set; }
    }
}
