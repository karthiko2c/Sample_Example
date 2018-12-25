using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RS.Entity.Models
{
    public partial class Roles
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int RoleId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}
