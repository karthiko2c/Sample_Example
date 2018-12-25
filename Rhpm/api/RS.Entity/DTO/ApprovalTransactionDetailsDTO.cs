using System;
using System.Collections.Generic;
using System.Text;

namespace RS.Entity.DTO
{
   public class ApprovalTransactionDetailsDTO
    {
        public string ApprovalActionName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? ActionPerformedDate { get; set; }
    }
}
