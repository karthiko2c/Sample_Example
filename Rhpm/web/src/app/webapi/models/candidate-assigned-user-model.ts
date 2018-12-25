/* tslint:disable */
import { ApprovalEventViewModel } from './approval-event-view-model';
import { CandidateViewModel } from './candidate-view-model';
import { UserViewModel } from './user-view-model';

/**
 */
export class CandidateAssignedUserModel {
    candidateAssignedUserId?: number;
    approvalEventId?: number;
    candidateId?: string;
    approvalEvent?: ApprovalEventViewModel;
    candidate?: CandidateViewModel;
    userId?: string;
    user?: UserViewModel;
}
