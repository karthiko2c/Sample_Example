using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RS.Entity.Models
{
    public partial class OpeningCandidates: BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int CandidateOpeningId { get; set; }

        public Guid CandidateId { get; set; }

        public Guid OpeningId { get; set; }

        public Openings Opening { get; set; }

        public Candidates Candidate { get; set; }
    }
}
