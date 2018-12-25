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
import { CandidateAssignedUserModel } from '../models/candidate-assigned-user-model';
import { ScheduleUserForCandidateModel } from '../models/schedule-user-for-candidate-model';


@Injectable()
export class CandidateService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   */
  ApiCandidateAddCandidatePostResponse(): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/Candidate/AddCandidate`,
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
  ApiCandidateAddCandidatePost(): Observable<IResult> {
    return this.ApiCandidateAddCandidatePostResponse().pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param candidateAssignedUserList - undefined
   */
  ApiCandidateAddUserForCandidatePostResponse(candidateAssignedUserList?: CandidateAssignedUserModel[]): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = candidateAssignedUserList;
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/Candidate/AddUserForCandidate`,
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
   * @param candidateAssignedUserList - undefined
   */
  ApiCandidateAddUserForCandidatePost(candidateAssignedUserList?: CandidateAssignedUserModel[]): Observable<IResult> {
    return this.ApiCandidateAddUserForCandidatePostResponse(candidateAssignedUserList).pipe(
      map(_r => _r.body)
    );
  }
  /**
   */
  ApiCandidateUpdateCandidatePutResponse(): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      "PUT",
      this.rootUrl + `/api/Candidate/UpdateCandidate`,
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
  ApiCandidateUpdateCandidatePut(): Observable<IResult> {
    return this.ApiCandidateUpdateCandidatePutResponse().pipe(
      map(_r => _r.body)
    );
  }
  /**
   */
  ApiCandidateGetAllCandidateGetResponse(): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Candidate/GetAllCandidate`,
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
  ApiCandidateGetAllCandidateGet(): Observable<IResult> {
    return this.ApiCandidateGetAllCandidateGetResponse().pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param id - undefined
   */
  ApiCandidateGetCandidateByIdGetResponse(id: string): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (id != null) __params = __params.set("id", id.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Candidate/GetCandidateById`,
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
   * @param id - undefined
   */
  ApiCandidateGetCandidateByIdGet(id: string): Observable<IResult> {
    return this.ApiCandidateGetCandidateByIdGetResponse(id).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param candidateId - undefined
   */
  ApiCandidateGetAssignedUsersByIdGetResponse(candidateId: string): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (candidateId != null) __params = __params.set("candidateId", candidateId.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Candidate/GetAssignedUsersById`,
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
  ApiCandidateGetAssignedUsersByIdGet(candidateId: string): Observable<IResult> {
    return this.ApiCandidateGetAssignedUsersByIdGetResponse(candidateId).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param candidateId - undefined
   */
  ApiCandidateGetScheduledUsersByIdGetResponse(candidateId: string): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (candidateId != null) __params = __params.set("candidateId", candidateId.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Candidate/GetScheduledUsersById`,
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
  ApiCandidateGetScheduledUsersByIdGet(candidateId: string): Observable<IResult> {
    return this.ApiCandidateGetScheduledUsersByIdGetResponse(candidateId).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param userId - undefined
   */
  ApiCandidateGetCandidatesCorrespondingToLoggedUserGetResponse(userId: string): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (userId != null) __params = __params.set("userId", userId.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Candidate/GetCandidatesCorrespondingToLoggedUser`,
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
   * @param userId - undefined
   */
  ApiCandidateGetCandidatesCorrespondingToLoggedUserGet(userId: string): Observable<IResult> {
    return this.ApiCandidateGetCandidatesCorrespondingToLoggedUserGetResponse(userId).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param candidateId - undefined
   */
  ApiCandidateApprovedForInterviewPutResponse(candidateId: string): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (candidateId != null) __params = __params.set("candidateId", candidateId.toString());
    let req = new HttpRequest<any>(
      "PUT",
      this.rootUrl + `/api/Candidate/ApprovedForInterview`,
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
  ApiCandidateApprovedForInterviewPut(candidateId: string): Observable<IResult> {
    return this.ApiCandidateApprovedForInterviewPutResponse(candidateId).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param scheduleUserForCandidateModelList - undefined
   */
  ApiCandidateAddUsersToConductInterviewPostResponse(scheduleUserForCandidateModelList?: ScheduleUserForCandidateModel[]): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = scheduleUserForCandidateModelList;
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/Candidate/AddUsersToConductInterview`,
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
   * @param scheduleUserForCandidateModelList - undefined
   */
  ApiCandidateAddUsersToConductInterviewPost(scheduleUserForCandidateModelList?: ScheduleUserForCandidateModel[]): Observable<IResult> {
    return this.ApiCandidateAddUsersToConductInterviewPostResponse(scheduleUserForCandidateModelList).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param file - undefined
   */
  ApiCandidateDownloadFilePostResponse(file?: string): Observable<HttpResponse<void>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (file != null) __params = __params.set("file", file.toString());
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/Candidate/DownloadFile`,
      __body,
      {
        headers: __headers,
        params: __params,
        responseType: 'text'
      });

    return this.http.request<any>(req).pipe(
      filter(_r => _r instanceof HttpResponse),
      map(_r => {
        let _resp = _r as HttpResponse<any>;
        let _body: void = null;
        
        return _resp.clone({body: _body}) as HttpResponse<void>;
      })
    );
  }

  /**
   * @param file - undefined
   */
  ApiCandidateDownloadFilePost(file?: string): Observable<void> {
    return this.ApiCandidateDownloadFilePostResponse(file).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param input - undefined
   */
  ApiCandidateGetOrganizationsOnInputChangedGetResponse(input?: string): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (input != null) __params = __params.set("input", input.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Candidate/GetOrganizationsOnInputChanged`,
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
   * @param input - undefined
   */
  ApiCandidateGetOrganizationsOnInputChangedGet(input?: string): Observable<IResult> {
    return this.ApiCandidateGetOrganizationsOnInputChangedGetResponse(input).pipe(
      map(_r => _r.body)
    );
  }}

export module CandidateService {
}
