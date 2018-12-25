import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { RoleService } from '../../../webapi/services';
import 'rxjs/Rx';

@Injectable()
export class RoleServiceApp {

    constructor(private apiRoleService: RoleService) { }

    getAllRoles(): Observable<any> {
        return this.apiRoleService.ApiRoleGetAllRoleGet().map(x => (x));
    }
}
