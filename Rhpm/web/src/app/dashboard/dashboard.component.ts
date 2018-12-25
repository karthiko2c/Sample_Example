import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppConstants } from '../shared/constant/constant.variable';
import decode from 'jwt-decode';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html'
})

export class DashboardComponent implements OnInit {
  loggedRole = null;
  constructor(private router: Router) {
  }

  ngOnInit() {
    this.setUserDashboard();
  }

  setUserDashboard() {
    let tokenPayload = '';
    const token = localStorage.getItem(AppConstants.AuthToken);
    // decode the token to get its payload
    if (token !== null) {
      tokenPayload = decode(token);
      this.loggedRole = tokenPayload[AppConstants.RoleClaim];
    }
  }

}
