import { ScheduleUserForCandidateModel } from '../../webapi/models/schedule-user-for-candidate-model';
import { UserViewModel } from '../../webapi/models/user-view-model';

export class ScheduleInterviewModel {
    finishedEventOrder?: number;
    nextApprovalEvent?: number;
    scheduleUserForCandidateModelList?: ScheduleUserForCandidateModel[];
    scheduledDate?: Date;
    users?: UserViewModel[];
    interviewScheduledTime?: Date[];
}
