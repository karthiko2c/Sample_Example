import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { UserService } from '../../../webapi/services';
import { HttpClient } from '@angular/common/http';
import { ApiConfiguration } from 'app/webapi/api-configuration';

@Injectable()
export class UserServiceApp {

    constructor(private apiUserService: UserService, private http: HttpClient, private apiConfig: ApiConfiguration) { }

    // createUser(userModel): Observable<any> {
    //     return this.apiUserService.ApiUserCreateUserPost(userModel).map(x => (x));
    // }

    GetUserDetails(): Observable<any> {
        return this.apiUserService.ApiUserGetUserDetailsGet().map(x => (x));
    }

    addUser(uri, user, fileToUpload: any): Observable<any> {
        const url = this.apiConfig.rootUrl + uri;
        const formdata = new FormData();
        formdata.append('userView', JSON.stringify(user));
        formdata.append('uploadProfile', fileToUpload);
        return this.http.post(url, formdata, { observe: 'response' }).map(x => (x));
    }

    updateUser(uri, user, fileToUpload: any): Observable<any> {
        const url = this.apiConfig.rootUrl + uri;
        const formdata = new FormData();
        formdata.append('userView', JSON.stringify(user));
        formdata.append('uploadProfile', fileToUpload);
        return this.http.put(url, formdata, { observe: 'response' }).map(x => (x));
    }

    getAllUsers(): Observable<any> {
        return this.apiUserService.ApiUserGetAllUserGet().map(x => (x));
    }

    getUserById(userId): Observable<any> {
    //    return this.apiUserService.ApiUserGetUserByIdGet(userId).map(x => (x));
    debugger;
      const url = this.apiConfig.rootUrl + '/api/User/GetUserById';
      const formdata = new FormData();
      formdata.append('userId', userId);
      return this.http.post(url, formdata, { responseType: 'blob' }).map(x => (x));
    }

    getUsersByRole(roleId): Observable<any> {
        return this.apiUserService.ApiUserGetUsersByRoleGet(roleId).map(x => (x));
    }

    GetUsersResults(searchAndSortModel): Observable<any> {
        return this.apiUserService.ApiUserGetUsersResultsPost(searchAndSortModel).map(x => (x));
    }
}
