/* tslint:disable */
import { Injectable } from '@angular/core';
import {
  HttpClient, HttpRequest, HttpResponse, 
  HttpHeaders, HttpParams } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { Observable } from 'rxjs/Observable';
import { map } from 'rxjs/operators/map';
import { filter } from 'rxjs/operators/filter';

import { IResult } from '../models/iresult';
import { ApprovalEventRoleViewModel } from '../models/approval-event-role-view-model';
import { EntityAndApprovalViewModel } from '../models/entity-and-approval-view-model';
import { CandidateListModel } from '../models/candidate-list-model';


@Injectable()
export class ApprovalService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * @param entityId - undefined
   * @param approvalId - undefined
   */
  ApiApprovalGetApprovalEventsGetResponse(params: ApprovalService.ApiApprovalGetApprovalEventsGetParams): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (params.entityId != null) __params = __params.set("entityId", params.entityId.toString());
    if (params.approvalId != null) __params = __params.set("approvalId", params.approvalId.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Approval/GetApprovalEvents`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      filter(_r => _r instanceof HttpResponse),
      map(_r => {
        let _resp = _r as HttpResponse<any>;
        let _body: IResult = null;
        _body = _resp.body as IResult
        return _resp.clone({body: _body}) as HttpResponse<IResult>;
      })
    );
  }

  /**
   * @param entityId - undefined
   * @param approvalId - undefined
   */
  ApiApprovalGetApprovalEventsGet(params: ApprovalService.ApiApprovalGetApprovalEventsGetParams): Observable<IResult> {
    return this.ApiApprovalGetApprovalEventsGetResponse(params).pipe(
      map(_r => _r.body)
    );
  }
  /**
   */
  ApiApprovalGetApprovalsGetResponse(): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Approval/GetApprovals`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      filter(_r => _r instanceof HttpResponse),
      map(_r => {
        let _resp = _r as HttpResponse<any>;
        let _body: IResult = null;
        _body = _resp.body as IResult
        return _resp.clone({body: _body}) as HttpResponse<IResult>;
      })
    );
  }

  /**
   */
  ApiApprovalGetApprovalsGet(): Observable<IResult> {
    return this.ApiApprovalGetApprovalsGetResponse().pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param entityId - undefined
   */
  ApiApprovalGetApprovalDetailsPostResponse(entityId: string): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (entityId != null) __params = __params.set("entityId", entityId.toString());
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/Approval/GetApprovalDetails`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      filter(_r => _r instanceof HttpResponse),
      map(_r => {
        let _resp = _r as HttpResponse<any>;
        let _body: IResult = null;
        _body = _resp.body as IResult
        return _resp.clone({body: _body}) as HttpResponse<IResult>;
      })
    );
  }

  /**
   * @param entityId - undefined
   */
  ApiApprovalGetApprovalDetailsPost(entityId: string): Observable<IResult> {
    return this.ApiApprovalGetApprovalDetailsPostResponse(entityId).pipe(
      map(_r => _r.body)
    );
  }
  /**
   */
  ApiApprovalGetAllApprovalEventRolesGetResponse(): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Approval/GetAllApprovalEventRoles`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      filter(_r => _r instanceof HttpResponse),
      map(_r => {
        let _resp = _r as HttpResponse<any>;
        let _body: IResult = null;
        _body = _resp.body as IResult
        return _resp.clone({body: _body}) as HttpResponse<IResult>;
      })
    );
  }

  /**
   */
  ApiApprovalGetAllApprovalEventRolesGet(): Observable<IResult> {
    return this.ApiApprovalGetAllApprovalEventRolesGetResponse().pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param openingId - undefined
   */
  ApiApprovalGetApprovalTransactionByEntityGetResponse(openingId: string): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (openingId != null) __params = __params.set("openingId", openingId.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Approval/GetApprovalTransactionByEntity`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      filter(_r => _r instanceof HttpResponse),
      map(_r => {
        let _resp = _r as HttpResponse<any>;
        let _body: IResult = null;
        _body = _resp.body as IResult
        return _resp.clone({body: _body}) as HttpResponse<IResult>;
      })
    );
  }

  /**
   * @param openingId - undefined
   */
  ApiApprovalGetApprovalTransactionByEntityGet(openingId: string): Observable<IResult> {
    return this.ApiApprovalGetApprovalTransactionByEntityGetResponse(openingId).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param roleId - undefined
   * @param approvalEventId - undefined
   */
  ApiApprovalGetApprovedUsersByRoleGetResponse(params: ApprovalService.ApiApprovalGetApprovedUsersByRoleGetParams): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (params.roleId != null) __params = __params.set("roleId", params.roleId.toString());
    if (params.approvalEventId != null) __params = __params.set("approvalEventId", params.approvalEventId.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Approval/GetApprovedUsersByRole`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      filter(_r => _r instanceof HttpResponse),
      map(_r => {
        let _resp = _r as HttpResponse<any>;
        let _body: IResult = null;
        _body = _resp.body as IResult
        return _resp.clone({body: _body}) as HttpResponse<IResult>;
      })
    );
  }

  /**
   * @param roleId - undefined
   * @param approvalEventId - undefined
   */
  ApiApprovalGetApprovedUsersByRoleGet(params: ApprovalService.ApiApprovalGetApprovedUsersByRoleGetParams): Observable<IResult> {
    return this.ApiApprovalGetApprovedUsersByRoleGetResponse(params).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param approvalEventRoleViewModel - undefined
   */
  ApiApprovalCreateEventRolePostResponse(approvalEventRoleViewModel?: ApprovalEventRoleViewModel): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = approvalEventRoleViewModel;
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/Approval/CreateEventRole`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      filter(_r => _r instanceof HttpResponse),
      map(_r => {
        let _resp = _r as HttpResponse<any>;
        let _body: IResult = null;
        _body = _resp.body as IResult
        return _resp.clone({body: _body}) as HttpResponse<IResult>;
      })
    );
  }

  /**
   * @param approvalEventRoleViewModel - undefined
   */
  ApiApprovalCreateEventRolePost(approvalEventRoleViewModel?: ApprovalEventRoleViewModel): Observable<IResult> {
    return this.ApiApprovalCreateEventRolePostResponse(approvalEventRoleViewModel).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param entityAndApprovalViewModel - undefined
   */
  ApiApprovalManageApprovalTransactionPutResponse(entityAndApprovalViewModel?: EntityAndApprovalViewModel): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = entityAndApprovalViewModel;
    let req = new HttpRequest<any>(
      "PUT",
      this.rootUrl + `/api/Approval/ManageApprovalTransaction`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      filter(_r => _r instanceof HttpResponse),
      map(_r => {
        let _resp = _r as HttpResponse<any>;
        let _body: IResult = null;
        _body = _resp.body as IResult
        return _resp.clone({body: _body}) as HttpResponse<IResult>;
      })
    );
  }

  /**
   * @param entityAndApprovalViewModel - undefined
   */
  ApiApprovalManageApprovalTransactionPut(entityAndApprovalViewModel?: EntityAndApprovalViewModel): Observable<IResult> {
    return this.ApiApprovalManageApprovalTransactionPutResponse(entityAndApprovalViewModel).pipe(
      map(_r => _r.body)
    );
  }
  /**
   */
  ApiApprovalGetDashboardDetailsGetResponse(): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Approval/GetDashboardDetails`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      filter(_r => _r instanceof HttpResponse),
      map(_r => {
        let _resp = _r as HttpResponse<any>;
        let _body: IResult = null;
        _body = _resp.body as IResult
        return _resp.clone({body: _body}) as HttpResponse<IResult>;
      })
    );
  }

  /**
   */
  ApiApprovalGetDashboardDetailsGet(): Observable<IResult> {
    return this.ApiApprovalGetDashboardDetailsGetResponse().pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param showType - undefined
   */
  ApiApprovalGetChartDetailsGetResponse(showType: number): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (showType != null) __params = __params.set("showType", showType.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Approval/GetChartDetails`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      filter(_r => _r instanceof HttpResponse),
      map(_r => {
        let _resp = _r as HttpResponse<any>;
        let _body: IResult = null;
        _body = _resp.body as IResult
        return _resp.clone({body: _body}) as HttpResponse<IResult>;
      })
    );
  }

  /**
   * @param showType - undefined
   */
  ApiApprovalGetChartDetailsGet(showType: number): Observable<IResult> {
    return this.ApiApprovalGetChartDetailsGetResponse(showType).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param candidateId - undefined
   */
  ApiApprovalGetNextEventOrderForCandidateGetResponse(candidateId: string): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (candidateId != null) __params = __params.set("candidateId", candidateId.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Approval/GetNextEventOrderForCandidate`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      filter(_r => _r instanceof HttpResponse),
      map(_r => {
        let _resp = _r as HttpResponse<any>;
        let _body: IResult = null;
        _body = _resp.body as IResult
        return _resp.clone({body: _body}) as HttpResponse<IResult>;
      })
    );
  }

  /**
   * @param candidateId - undefined
   */
  ApiApprovalGetNextEventOrderForCandidateGet(candidateId: string): Observable<IResult> {
    return this.ApiApprovalGetNextEventOrderForCandidateGetResponse(candidateId).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param candidateId - undefined
   */
  ApiApprovalGetApprovalEventsOfUserForCandidateGetResponse(candidateId: string): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (candidateId != null) __params = __params.set("candidateId", candidateId.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Approval/GetApprovalEventsOfUserForCandidate`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      filter(_r => _r instanceof HttpResponse),
      map(_r => {
        let _resp = _r as HttpResponse<any>;
        let _body: IResult = null;
        _body = _resp.body as IResult
        return _resp.clone({body: _body}) as HttpResponse<IResult>;
      })
    );
  }

  /**
   * @param candidateId - undefined
   */
  ApiApprovalGetApprovalEventsOfUserForCandidateGet(candidateId: string): Observable<IResult> {
    return this.ApiApprovalGetApprovalEventsOfUserForCandidateGetResponse(candidateId).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param candidate - undefined
   */
  ApiApprovalCheckForStartInterviewPostResponse(candidate?: CandidateListModel): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = candidate;
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/Approval/CheckForStartInterview`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'json'
      });

    return this.http.request<any>(req).pipe(
      filter(_r => _r instanceof HttpResponse),
      map(_r => {
        let _resp = _r as HttpResponse<any>;
        let _body: IResult = null;
        _body = _resp.body as IResult
        return _resp.clone({body: _body}) as HttpResponse<IResult>;
      })
    );
  }

  /**
   * @param candidate - undefined
   */
  ApiApprovalCheckForStartInterviewPost(candidate?: CandidateListModel): Observable<IResult> {
    return this.ApiApprovalCheckForStartInterviewPostResponse(candidate).pipe(
      map(_r => _r.body)
    );
  }}

export module ApprovalService {
  export interface ApiApprovalGetApprovalEventsGetParams {
    entityId: string;
    approvalId: number;
  }
  export interface ApiApprovalGetApprovedUsersByRoleGetParams {
    roleId: number;
    approvalEventId: number;
  }
}
