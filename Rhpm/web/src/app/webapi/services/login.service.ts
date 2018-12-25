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
import { UserLoginModel } from '../models/user-login-model';
import { ChangePassword } from '../models/change-password';


@Injectable()
export class LoginService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * @param loginModel - undefined
   */
  ApiLoginLoginUserPostResponse(loginModel?: UserLoginModel): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = loginModel;
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/Login/LoginUser`,
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
   * @param loginModel - undefined
   */
  ApiLoginLoginUserPost(loginModel?: UserLoginModel): Observable<IResult> {
    return this.ApiLoginLoginUserPostResponse(loginModel).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param changePassword - undefined
   */
  ApiLoginChangePasswordPutResponse(changePassword?: ChangePassword): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = changePassword;
    let req = new HttpRequest<any>(
      "PUT",
      this.rootUrl + `/api/Login/ChangePassword`,
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
   * @param changePassword - undefined
   */
  ApiLoginChangePasswordPut(changePassword?: ChangePassword): Observable<IResult> {
    return this.ApiLoginChangePasswordPutResponse(changePassword).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param userName - undefined
   */
  ApiLoginForgotPasswordPostResponse(userName?: string): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = userName;
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/Login/ForgotPassword`,
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
   * @param userName - undefined
   */
  ApiLoginForgotPasswordPost(userName?: string): Observable<IResult> {
    return this.ApiLoginForgotPasswordPostResponse(userName).pipe(
      map(_r => _r.body)
    );
  }
  /**
   */
  ApiLoginUpdateUserProfilePutResponse(): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      "PUT",
      this.rootUrl + `/api/Login/UpdateUserProfile`,
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
  ApiLoginUpdateUserProfilePut(): Observable<IResult> {
    return this.ApiLoginUpdateUserProfilePutResponse().pipe(
      map(_r => _r.body)
    );
  }}

export module LoginService {
}
