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
import { SkillViewModel } from '../models/skill-view-model';
import { SearchAndSortModel } from '../models/search-and-sort-model';


@Injectable()
export class SkillService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * @param skillView - undefined
   */
  ApiSkillCreateSkillPostResponse(skillView?: SkillViewModel): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = skillView;
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/Skill/CreateSkill`,
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
   * @param skillView - undefined
   */
  ApiSkillCreateSkillPost(skillView?: SkillViewModel): Observable<IResult> {
    return this.ApiSkillCreateSkillPostResponse(skillView).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param skillView - undefined
   */
  ApiSkillUpdateSkillPutResponse(skillView?: SkillViewModel): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = skillView;
    let req = new HttpRequest<any>(
      "PUT",
      this.rootUrl + `/api/Skill/UpdateSkill`,
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
   * @param skillView - undefined
   */
  ApiSkillUpdateSkillPut(skillView?: SkillViewModel): Observable<IResult> {
    return this.ApiSkillUpdateSkillPutResponse(skillView).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param skillView - undefined
   */
  ApiSkillDeleteSkillPutResponse(skillView?: SkillViewModel): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = skillView;
    let req = new HttpRequest<any>(
      "PUT",
      this.rootUrl + `/api/Skill/DeleteSkill`,
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
   * @param skillView - undefined
   */
  ApiSkillDeleteSkillPut(skillView?: SkillViewModel): Observable<IResult> {
    return this.ApiSkillDeleteSkillPutResponse(skillView).pipe(
      map(_r => _r.body)
    );
  }
  /**
   */
  ApiSkillGetAllSkillGetResponse(): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Skill/GetAllSkill`,
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
  ApiSkillGetAllSkillGet(): Observable<IResult> {
    return this.ApiSkillGetAllSkillGetResponse().pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param id - undefined
   */
  ApiSkillGetSkillByIdGetResponse(id: number): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    if (id != null) __params = __params.set("id", id.toString());
    let req = new HttpRequest<any>(
      "GET",
      this.rootUrl + `/api/Skill/GetSkillById`,
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
  ApiSkillGetSkillByIdGet(id: number): Observable<IResult> {
    return this.ApiSkillGetSkillByIdGetResponse(id).pipe(
      map(_r => _r.body)
    );
  }
  /**
   * @param searchAndSortModel - undefined
   */
  ApiSkillGetSkillsResultsPostResponse(searchAndSortModel?: SearchAndSortModel): Observable<HttpResponse<IResult>> {
    let __params = this.newParams();
    let __headers = new HttpHeaders();
    let __body: any = null;
    __body = searchAndSortModel;
    let req = new HttpRequest<any>(
      "POST",
      this.rootUrl + `/api/Skill/GetSkillsResults`,
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
  ApiSkillGetSkillsResultsPost(searchAndSortModel?: SearchAndSortModel): Observable<IResult> {
    return this.ApiSkillGetSkillsResultsPostResponse(searchAndSortModel).pipe(
      map(_r => _r.body)
    );
  }}

export module SkillService {
}
