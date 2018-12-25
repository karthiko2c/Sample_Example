import { Injectable } from '@angular/core';
import { Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { LoginService } from '../../webapi/services';
import { UserService } from '../../webapi/services';

@Injectable()
export class LoginServiceApp {

    constructor(private http: HttpClient, private apiLoginService: LoginService) { }

    userLogin(userLoginModel): Observable<any> {
        return this.apiLoginService.ApiLoginLoginUserPost(userLoginModel).map(x => (x));
    }
    
    userForgotpassword(userForgotpasswordModel): Observable<any> {
        return this.apiLoginService.ApiLoginForgotPasswordPost(userForgotpasswordModel).map(x => (x));
    }
}
