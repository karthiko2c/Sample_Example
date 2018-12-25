import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

// Constants
import { AppConstants } from '../constant/constant.variable';

@Injectable()
export class AuthService {
  constructor(public jwtHelper: JwtHelperService) { }
  // ...
  public isAuthenticated(): boolean {
    const token = localStorage.getItem(AppConstants.AuthToken);
    // Check whether the token is expired and return
    // true or false
    if (token !== null) { return !this.jwtHelper.isTokenExpired(token); }
    return false;
  }
}
