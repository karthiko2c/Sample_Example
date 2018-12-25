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
import { QualificationViewModel } from '../models/qualification-view-model';
import { SearchAndSortModel } from '../models/search-and-sort-model';


@Injectable()
export class QualificatonService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * @param qualificationView - undefined
   */
  ApiQualificatonCreateQualificationPostResponse(qualificationView?: QualificationViewModel): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = qualificationView;
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/Qualificaton/CreateQualification`,
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
   * @param qualificationView - undefined
   */
  ApiQualificatonCreateQualificationPost(qualificationView?: QualificationViewModel): Observable<IResult> {
    return this.ApiQualificatonCreateQualificationPostResponse(qualificationView).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param qualificationView - undefined
   */
  ApiQualificatonUpdateQualificationPutResponse(qualificationView?: QualificationViewModel): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = qualificationView;
    let req = new HttpRequest<any>(
      "PUT",
      this.rootUrl + `/api/Qualificaton/UpdateQualification`,
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
   * @param qualificationView - undefined
   */
  ApiQualificatonUpdateQualificationPut(qualificationView?: QualificationViewModel): Observable<IResult> {
    return this.ApiQualificatonUpdateQualificationPutResponse(qualificationView).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param qualificationView - undefined
   */
  ApiQualificatonDeleteQualificationPutResponse(qualificationView?: QualificationViewModel): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = qualificationView;
    let req = new HttpRequest<any>(
      "PUT",
      this.rootUrl + `/api/Qualificaton/DeleteQualification`,
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
   * @param qualificationView - undefined
   */
  ApiQualificatonDeleteQualificationPut(qualificationView?: QualificationViewModel): Observable<IResult> {
    return this.ApiQualificatonDeleteQualificationPutResponse(qualificationView).pipe(
      map(_r => _r.body)
    );
  }
  /**
   */
  ApiQualificatonGetAllQualificationGetResponse(): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Qualificaton/GetAllQualification`,
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
  ApiQualificatonGetAllQualificationGet(): Observable<IResult> {
    return this.ApiQualificatonGetAllQualificationGetResponse().pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param id - undefined
   */
  ApiQualificatonGetQualificationlByIdGetResponse(id: number): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (id != null) __params = __params.set("id", id.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Qualificaton/GetQualificationlById`,
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
  ApiQualificatonGetQualificationlByIdGet(id: number): Observable<IResult> {
    return this.ApiQualificatonGetQualificationlByIdGetResponse(id).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param searchAndSortModel - undefined
   */
  ApiQualificatonGetQualificationsResultsPostResponse(searchAndSortModel?: SearchAndSortModel): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = searchAndSortModel;
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/Qualificaton/GetQualificationsResults`,
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
   * @param searchAndSortModel - undefined
   */
  ApiQualificatonGetQualificationsResultsPost(searchAndSortModel?: SearchAndSortModel): Observable<IResult> {
    return this.ApiQualificatonGetQualificationsResultsPostResponse(searchAndSortModel).pipe(
      map(_r => _r.body)
    );
  }}

export module QualificatonService {
}
