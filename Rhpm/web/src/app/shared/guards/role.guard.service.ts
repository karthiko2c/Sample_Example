import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot } from '@angular/router';
import decode from 'jwt-decode';

import { AuthService } from './auth.service';

// constants
import { AppConstants } from '../constant/constant.variable';
import { isNullOrUndefined } from 'util';


@Injectable()
export class RoleGuardService implements CanActivate {

  constructor(public auth: AuthService, public router: Router) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    // this will be passed from the route config
    // on the data property
    let tokenPayload = '';
    let expectedRole: any[] = [];
    expectedRole = route.data.expectedRole;
    const token = localStorage.getItem(AppConstants.AuthToken);
    // decode the token to get its payload
    if (!isNullOrUndefined(token)) { tokenPayload = decode(token); }
    if (this.auth.isAuthenticated()) {
      if (expectedRole != null) {
        for (const role in expectedRole) {
          if (tokenPayload[AppConstants.RoleClaim] === expectedRole[role]) {
            return true;
          }
        }
      } else {
        return true;
      }
    }
    this.router.navigate(['login']);
    return false;
  }
}

