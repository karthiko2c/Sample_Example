import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppConstants } from '../constant/constant.variable';
import * as $ from 'jquery/dist/jquery.min.js';
import { isNullOrUndefined } from 'util';
import decode from 'jwt-decode';
import { SharedService } from 'app/shared/service/shared.service';
import { UserServiceApp } from 'app/admin/users/shared/user.serviceApp';
import { Status } from 'app/app.enum';
import { DisplayMessageService } from 'app/shared/toastr/display.message.service';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html'
})

export class HeaderComponent implements OnInit {

    name: string;
    userId: any;
    intialOfFirstName: any;
    intialOfLastName: any;

    constructor(private router: Router, private sharedSevice: SharedService, private userService: UserServiceApp,
        private msgService: DisplayMessageService) {
        sharedSevice.userDetail.subscribe(user => this.updateUser(user));
    }

    ngOnInit() {
        debugger;
        let tokenPayload = '';
        const token = localStorage.getItem(AppConstants.AuthToken);
        if (!isNullOrUndefined(token)) { tokenPayload = decode(token); }
        this.userId = tokenPayload[AppConstants.IdClaim];
        this.getUser(this.userId);
    }

    getUser(userId) {
        this.userService.getUserById(this.userId).subscribe(
            (data) => {
                debugger;
                if (data.status === Status.Success) {
                    this.updateUser(data.body);
                } else {
                    this.msgService.showError('Error');
                }
            }
        );
    }

    updateUser(user) {
        this.name = user.firstName + ' ' + user.lastName;
        this.splitName(this.name);
    }

    splitName(name) {
        const names = this.name.split(' ');
        this.intialOfFirstName = names[0].charAt(0);
        this.intialOfLastName = names[1].charAt(0);
    }


    logout() {
        localStorage.removeItem(AppConstants.AuthToken);
        localStorage.clear();
        this.router.navigate(['login']);
    }

    toggleLeft() {
        if (!$('.side-bar-main').hasClass('side-bar')) {
            $('.side-bar-main').toggleClass('side-left', 'slow');
        }

        if (!$('div').hasClass('.content-wrapper')) {
            $('div.content-wrapper').toggleClass('wrap-left', 'slow');
        }
    }
}
