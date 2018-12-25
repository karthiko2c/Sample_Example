using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RS.Entity.Models
{
    public partial class UserRoles : BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserRolesId { get; set; }

        public Users user { get; set; }

        public Guid UserId { get; set; }

        public int RoleId { get; set; }

        public Roles Role { get; set; }
    }
}
