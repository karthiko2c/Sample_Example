using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RS.Entity.Models
{
    public partial class Organizations : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int OrganizationId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}
