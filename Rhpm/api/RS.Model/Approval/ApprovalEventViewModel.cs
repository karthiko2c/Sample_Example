using RS.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace RS.ViewModel.Approval
{
    public class ApprovalEventViewModel
    {
        public int ApprovalEventId { get; set; }

        public string ApprovalEventName { get; set; }

        public int ApprovalEventOrder { get; set; }

        public int ApprovalId { get; set; }

        public List<ApprovalActionViewModel> ApprovalActions { get; set; }

        public List<UserViewModel> Users { get; set; }

        // public List<ApprovalEventRoles> ApprovalEventRoles { get; set; }
    }
}
