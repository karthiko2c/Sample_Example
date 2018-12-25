import { Injectable } from '@angular/core';
import { Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import 'rxjs/Rx';
import { ApprovalService } from '../../../webapi/services/approval.service';

@Injectable()
export class UserEventRoleServiceApp {

    constructor(private http: HttpClient, private apiApprovalService: ApprovalService) { }

    CreateUserEventRole(approvalEventRoleModel): Observable<any> {
        return this.apiApprovalService.ApiApprovalCreateEventRolePost(approvalEventRoleModel).map(x => (x));
    }

    getAllUserEventRole(): Observable<any> {
        return this.apiApprovalService.ApiApprovalGetAllApprovalEventRolesGet().map(x => (x));
    }

    getApprovedUsers(approvalRole): Observable<any> {
        return this.apiApprovalService.ApiApprovalGetApprovedUsersByRoleGet(approvalRole).map(x => (x));
    }

}
