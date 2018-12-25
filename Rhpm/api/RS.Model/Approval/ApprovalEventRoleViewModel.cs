using RS.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace RS.ViewModel.Approval
{
    public class ApprovalEventRoleViewModel
    {
        public int ApprovalEventRoleId { get; set; }

        public int ApprovalEventId { get; set; }

        public String ApprovalEvent { get; set; }

        public int RoleId { get; set; }

        public String Role { get; set; }

        public Guid UserId { get; set; }

        public String User { get; set; }

        public List<UserViewModel> Users { get; set; }

    }
}
