/* tslint:disable */
import { UserViewModel } from './user-view-model';
import { ApprovalEventViewModel } from './approval-event-view-model';
import { CandidateViewModel } from './candidate-view-model';

/**
 */
export class ScheduleUserForCandidateModel {
    interviewScheduledDate?: string;
    scheduleUserForCandidateId?: number;
    candidateId?: string;
    userId?: string;
    scheduledOn?: string;
    approvalEventId?: number;
    interviewScheduledTime?: string;
    isFinished?: boolean;
    user?: UserViewModel;
    approvalEvent?: ApprovalEventViewModel;
    candidate?: CandidateViewModel;
}
