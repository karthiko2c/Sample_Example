/* tslint:disable */
import { ApprovalActionViewModel } from './approval-action-view-model';
import { UserViewModel } from './user-view-model';

/**
 */
export class ApprovalEventViewModel {
    approvalEventId?: number;
    approvalEventName?: string;
    approvalEventOrder?: number;
    approvalId?: number;
    approvalActions?: ApprovalActionViewModel[];
    users?: UserViewModel[];
}
