/* tslint:disable */
import { UserViewModel } from './user-view-model';
import { ApprovalActionViewModel } from './approval-action-view-model';

/**
 */
export class ApprovalTransactionViewModel {
    approvalActionId?: number;
    approvalTransactionId?: number;
    approvalEventOrder?: number;
    eventOrderNumber?: number;
    nextEventOrderNumber?: number;
    permissibleEvent?: number;
    approvalId?: number;
    entityId?: string;
    entityType?: number;
    comments?: string;
    action?: string;
    user?: UserViewModel;
    approvalActions?: ApprovalActionViewModel[];
}
