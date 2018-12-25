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
import { SearchAndSortModel } from '../models/search-and-sort-model';


@Injectable()
export class UserService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   */
  ApiUserGetUserDetailsGetResponse(): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/User/GetUserDetails`,
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
  ApiUserGetUserDetailsGet(): Observable<IResult> {
    return this.ApiUserGetUserDetailsGetResponse().pipe(
      map(_r => _r.body)
    );
  }
  /**
   */
  ApiUserCreateUserPostResponse(): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/User/CreateUser`,
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
  ApiUserCreateUserPost(): Observable<IResult> {
    return this.ApiUserCreateUserPostResponse().pipe(
      map(_r => _r.body)
    );
  }
  /**
   */
  ApiUserUpdateUserPutResponse(): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      "PUT",
      this.rootUrl + `/api/User/UpdateUser`,
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
  ApiUserUpdateUserPut(): Observable<IResult> {
    return this.ApiUserUpdateUserPutResponse().pipe(
      map(_r => _r.body)
    );
  }
  /**
   */
  ApiUserGetAllUserGetResponse(): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/User/GetAllUser`,
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
  ApiUserGetAllUserGet(): Observable<IResult> {
    return this.ApiUserGetAllUserGetResponse().pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param userId - undefined
   */
  ApiUserGetUserByIdGetResponse(userId: string): Observable<HttpResponse<void>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (userId != null) __params = __params.set("userId", userId.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/User/GetUserById`,
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
   * @param userId - undefined
   */
    ApiUserGetUserByIdGet(userId: string): Observable<void> {
    return this.ApiUserGetUserByIdGetResponse(userId).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param id - undefined
   */
  ApiUserGetUsersByRoleGetResponse(id: number): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (id != null) __params = __params.set("id", id.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/User/GetUsersByRole`,
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
  ApiUserGetUsersByRoleGet(id: number): Observable<IResult> {
    return this.ApiUserGetUsersByRoleGetResponse(id).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param searchAndSortModel - undefined
   */
  ApiUserGetUsersResultsPostResponse(searchAndSortModel?: SearchAndSortModel): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = searchAndSortModel;
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/User/GetUsersResults`,
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
  ApiUserGetUsersResultsPost(searchAndSortModel?: SearchAndSortModel): Observable<IResult> {
    return this.ApiUserGetUsersResultsPostResponse(searchAndSortModel).pipe(
      map(_r => _r.body)
    );
  }}

export module UserService {
}
