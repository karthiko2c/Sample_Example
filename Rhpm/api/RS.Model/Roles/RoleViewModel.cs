
using System;
using System.ComponentModel.DataAnnotations;

namespace RS.ViewModel.Roles
{
    public class RoleViewModel
    {
        public int RoleId { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }
       
    }
}
