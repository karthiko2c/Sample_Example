import { Injectable } from '@angular/core';
import { Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { LoginService } from '../../webapi/services/login.service';
import { UserService } from '../../webapi/services/user.service';
import { ApiConfiguration } from 'app/webapi/api-configuration';

@Injectable()
export class ChangepasswordServiceApp {
    constructor(private http: HttpClient, private apiChangepasswordService: LoginService, private apiConfig: ApiConfiguration) { }

    userChangepassword(changepasswordModel): Observable<any> {
        return this.apiChangepasswordService.ApiLoginChangePasswordPut(changepasswordModel).map(x => (x));
    }

    userEditprofile(uri, user, fileToUpload: any): Observable<any> {
        const url = this.apiConfig.rootUrl + uri;
        const formdata = new FormData();
        formdata.append('userView', JSON.stringify(user));
        formdata.append('uploadProfile', fileToUpload);
        return this.http.put(url, formdata, { observe: 'response' }).map(x => (x));
    }
}
