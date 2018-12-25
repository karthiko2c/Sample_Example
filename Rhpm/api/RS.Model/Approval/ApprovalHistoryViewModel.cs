using System;
using System.Collections.Generic;
using System.Text;

namespace RS.ViewModel.Approval
{
    class ApprovalHistoryViewModel
    {
            public string UserName { get; set; }

            public int EventOrdetNumber { get; set; }

            public string ApprovalActionId { get; set; }

            public string CreatedBy { get; set; } 

            public DateTime CreatedDate { get; set; }
    }
}
