using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RS.Common.Enums;

namespace RS.Entity.Models
{
    public partial class OpeningSkills : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int OpeningSkillId { get; set; }

        public Guid OpeningId { get; set; }

        public int SkillId { get; set; }

        [Required]
        public OpeningSkillType SkillType { get; set; }

        public Openings Opening { get; set; }

        public Skills Skill { get; set; }
    }
}
