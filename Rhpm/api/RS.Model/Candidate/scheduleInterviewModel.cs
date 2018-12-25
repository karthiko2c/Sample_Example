using RS.ViewModel.Approval;
using RS.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace RS.ViewModel.Candidate
{
    public class ScheduleInterviewModel
    {
        public int FinishedEventOrder { get; set; }

        public int NextApprovalEvent { get; set; }

        public List<ScheduleUserForCandidateModel> ScheduleUserForCandidateModelList { get; set; }

        public DateTime ScheduledDate { get; set; }

        public List<UserViewModel> Users { get; set; }

        public List<DateTime> InterviewScheduledTime { get; set; }

    }
}
