import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ApprovalService } from '../../webapi/services/approval.service';
import 'rxjs/Rx';

@Injectable()
export class ApprovalServiceApp {

  constructor(private apiApprovalService: ApprovalService) { }

  getApprovalEventsById(approvalEventandTransaction): Observable<any> {
    return this.apiApprovalService.ApiApprovalGetApprovalEventsGet(approvalEventandTransaction).map(x => (x));
  }

  getApprovals(): Observable<any> {
    return this.apiApprovalService.ApiApprovalGetApprovalsGet().map((x) => x);
  }

  getApprovalDetails(userId):Observable<any>{
    return this.apiApprovalService.ApiApprovalGetApprovalDetailsPost(userId).map(x => (x));
}
  getApprovalTransactionByEntity(entityId): Observable<any> {
    return this.apiApprovalService.ApiApprovalGetApprovalTransactionByEntityGet(entityId).map(x => (x));
  }

  manageApprovalTransaction(entityAndApprovalModel): Observable<any> {
    return this.apiApprovalService.ApiApprovalManageApprovalTransactionPut(entityAndApprovalModel).map(x => (x));
  }

  getDashboardDetails(): Observable<any> {
    return this.apiApprovalService.ApiApprovalGetDashboardDetailsGet().map(x => (x));
  }

  getChartDetails(showType): Observable<any> {
    return this.apiApprovalService.ApiApprovalGetChartDetailsGet(showType).map(x => (x));
  }

  checkForStartInterview(candidate): Observable<any> {
    return this.apiApprovalService.ApiApprovalCheckForStartInterviewPost(candidate).map(x => (x));
  }

  getApprovalEventsOfUserForCandidate(candidate): Observable<any> {
    return this.apiApprovalService.ApiApprovalGetApprovalEventsOfUserForCandidateGet(candidate).map(x => (x));
  }

}
