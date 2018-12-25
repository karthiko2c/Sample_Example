using System;
using System.ComponentModel.DataAnnotations;

namespace RS.ViewModel.User
{
    public class LoginCredential
    {
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(100)]
        public string Password { get; set; }

        [MaxLength(4)]
        public string PIN { get; set; }
        public string IMEI { get; set; }
        public bool IsPIN { get; set; }
        public bool IsOldPIN { get; set; }
        public Guid UserId { get; set; }
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public int UserRole { get; set; }
        public DateTime LastLoginTime { get; set; }
        public string ResetPassword { get; set; }
        public bool RememberMe { get; set; }
        public string DeviceType { get; set; }
        public string DeviceToken { get; set; }

        public string OldPIN { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
