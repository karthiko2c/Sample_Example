
using System;
using System.ComponentModel.DataAnnotations;

namespace RS.ViewModel.Roles
{
    public class RoleModel
    {
        public int RoleId { get; set; }

        [Required, MaxLength(200)]
        public string RoleName { get; set; }

        [Required, MaxLength(200)]
        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public Guid DeletedBy { get; set; }

        public DateTime DeletedDateTime { get; set; }

        public Guid ModifiedBy { get; set; }

        public DateTime ModifiedDateTime{ get; set; }

    }
}
