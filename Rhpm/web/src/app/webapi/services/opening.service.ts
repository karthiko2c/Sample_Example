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
import { EntityAndApprovalViewModel } from '../models/entity-and-approval-view-model';


@Injectable()
export class OpeningService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * @param entityAndApprovalViewModel - undefined
   */
  ApiOpeningInsertOrUpdateOpeningPostResponse(entityAndApprovalViewModel?: EntityAndApprovalViewModel): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = entityAndApprovalViewModel;
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/Opening/InsertOrUpdateOpening`,
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
  ApiOpeningInsertOrUpdateOpeningPost(entityAndApprovalViewModel?: EntityAndApprovalViewModel): Observable<IResult> {
    return this.ApiOpeningInsertOrUpdateOpeningPostResponse(entityAndApprovalViewModel).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param userId - undefined
   */
  ApiOpeningGetOpeningsCorrespondingToLoggedUserGetResponse(userId: string): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (userId != null) __params = __params.set("userId", userId.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Opening/GetOpeningsCorrespondingToLoggedUser`,
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
  ApiOpeningGetOpeningsCorrespondingToLoggedUserGet(userId: string): Observable<IResult> {
    return this.ApiOpeningGetOpeningsCorrespondingToLoggedUserGetResponse(userId).pipe(
      map(_r => _r.body)
    );
  }
  /**
   */
  ApiOpeningGetAllOpeningGetResponse(): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Opening/GetAllOpening`,
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
  ApiOpeningGetAllOpeningGet(): Observable<IResult> {
    return this.ApiOpeningGetAllOpeningGetResponse().pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param id - undefined
   */
  ApiOpeningGetOpeningByIdGetResponse(id: string): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (id != null) __params = __params.set("id", id.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Opening/GetOpeningById`,
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
  ApiOpeningGetOpeningByIdGet(id: string): Observable<IResult> {
    return this.ApiOpeningGetOpeningByIdGetResponse(id).pipe(
      map(_r => _r.body)
    );
  }}

export module OpeningService {
}
