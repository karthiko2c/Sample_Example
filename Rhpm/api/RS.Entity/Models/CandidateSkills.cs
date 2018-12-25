using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS.Entity.Models
{
    public partial class CandidateSkills 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int CandidateSkillId { get; set; }
        public Guid CandidateId { get; set; }
        public int SkillId { get; set; }

        public Candidate Candidate { get; set; }
        public Skills Skill { get; set; }
    }
}
