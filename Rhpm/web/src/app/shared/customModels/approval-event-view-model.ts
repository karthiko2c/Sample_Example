import { ApprovalActionViewModel, UserViewModel } from '../../webapi/models';
import { UserModel } from './user-model';

export class ApprovalEventViewModel {
    approvalEventId?: number;
    approvalEventName?: string;
    approvalEventOrder?: number;
    approvalId?: number;
    approvalActions?: ApprovalActionViewModel[];
    users?: UserModel[];
    userCompletedInterview?: UserViewModel;
    interviewConductedOn?: Date;
}
