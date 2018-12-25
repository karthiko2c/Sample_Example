using System;

namespace RS.Common.CommonData
{
    public class UserClaim
    {
        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public Guid UserId { get; set; }

        public int RoleId { get; set; }
    }
}
