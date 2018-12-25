import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { LoginServiceApp } from './shared/login.serviceApp';
import { isNullOrUndefined } from 'util';
import { UserViewModel } from '../webapi/models';
import { LoginComponent } from './login.component';
import { Status } from '../app.enum';

@Component({
    selector: 'app-forgotpassword',
    templateUrl: 'forgotpassword.component.html',
})
export class ForgotpasswordComponent implements OnInit {
    userName: string;

    ForgotpasswordModel: UserViewModel = {} as UserViewModel;
    constructor(
        private loginServiceApp: LoginServiceApp,
        private router: Router) {
    }
    ngOnInit() {
    }
    onSubmit(forgotpasswordform) {
        if (forgotpasswordform.valid) {
            this.loginServiceApp.userForgotpassword(this.userName).subscribe(
                (data) => {
                    if (!isNullOrUndefined(data) && data.status === Status.Success) {
                        this.router.navigate(['Login']);
                    } else {
                        (data) => {
                        }
                    }
                }
            );
        }
    }
    goBack() {
        this.router.navigate(['login']);
    }
}
