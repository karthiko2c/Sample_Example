import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { HeaderComponent } from './../shared/header/header.component';
import { DetailModule } from './shared/detail.module';
import { ChangepasswordServiceApp } from './shared/changepassword.serviceApp';
import { isNullOrUndefined } from 'util';
import { Status } from '../app.enum';
import { ChangePassword } from '../webapi/models';
import { DisplayMessageService } from '../shared/toastr/display.message.service';
import 'rxjs/add/observable/of';
import { UtilityService } from '../shared/utility/utility.service';

@Component({
    selector: 'app-changepassword',
    templateUrl: 'changepassword.component.html'
})
export class ChangepasswordComponent implements OnInit {
    ChangepasswordModel: ChangePassword = {} as ChangePassword;
    changepasswordform: any;
    confirm: string;

    constructor(private router: Router, private changepasswordServiceApp: ChangepasswordServiceApp,
        private displayMessage: DisplayMessageService, private utilityService: UtilityService) {
    }
    onSubmit(changepasswordform) {
        if (changepasswordform.valid) {
            if (this.ChangepasswordModel.newPassword === this.confirm) {
                this.ChangepasswordModel.newPassword = this.utilityService.encrypt(this.ChangepasswordModel.newPassword);
                this.changepasswordServiceApp.userChangepassword(this.ChangepasswordModel).subscribe(
                    (data) => {
                        this.displayMessage.showSuccess('CHANGEPASSWORD.CHANGEPASSWORDSUCCESS');
                    });
            } else {
                this.displayMessage.showWarning('USER.PASSWORDMISMATCH');
            }
        }
        changepasswordform.reset();
    }
    ngOnInit() {
    }
    goBack() {
        this.router.navigate(['Dashboard']);
    }
}
