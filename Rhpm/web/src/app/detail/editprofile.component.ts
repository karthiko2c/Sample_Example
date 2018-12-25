import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ChangepasswordServiceApp } from './shared/changepassword.serviceApp';
import { DisplayMessageService } from '../shared/toastr/display.message.service';
import { UserModel } from '../shared/customModels/user-model';
import { UserServiceApp } from '../admin/users/shared/user.serviceApp';
import { UtilityService } from 'app/shared/utility/utility.service';
import { AppConstants } from 'app/shared/constant/constant.variable';
import { Status } from 'app/app.enum';
import { SharedService } from 'app/shared/service/shared.service';

@Component({
    selector: 'app-editprofile',
    templateUrl: 'editprofile.component.html'
})
export class EditprofileComponent implements OnInit {

    userModel: UserModel = {} as UserModel;
    imageChangedEvent: any = '';
    croppedImage: any = '';
    uploadedProfile: any;
    fileName: any;
    submitted = false;

    constructor(private router: Router, private userServiceApp: UserServiceApp, private changepasswordServiceApp: ChangepasswordServiceApp,
        private msgService: DisplayMessageService, private utilityService: UtilityService,
        private sharedService: SharedService) {
    }

    ngOnInit() {
        this.GetUserDetails();
    }

    onSubmit(editprofileform) {
        this.submitted = true;
        if (editprofileform.valid) {
            this.changepasswordServiceApp.userEditprofile(AppConstants.uriForUpdateUserProfile, this.userModel, this.uploadedProfile)
                .subscribe(
                (data) => {
                    if (data.body.status === Status.Success) {
                        this.sharedService.updateUserDetail(this.userModel);
                        this.msgService.showSuccess('EDITPROFILE.UPDATEPROFILESUCCESS');
                        this.router.navigate(['Dashboard']);
                    } else if (data.status === Status.Fail && data.message === AppConstants.duplicateUserName) {
                        this.msgService.showInfo('EDITPROFILE.DUPLICATEEMAIL');
                    } else if (data.status === Status.Fail && data.message === AppConstants.duplicateEmail) {
                        this.msgService.showInfo('EDITPROFILE.DUPLICATEUSER');
                    } else {
                        this.msgService.showError('Error');
                    }
                });
        }
    }

    fileChangeEvent(event: any): void {
        this.imageChangedEvent = event;
        this.fileName = event.target.files[0].name;
    }

    imageCropped(image: string) {
        this.croppedImage = image;
        this.uploadedProfile = this.utilityService.dataURLtoFile(this.croppedImage, this.fileName);
    }

    GetUserDetails() {
        this.userServiceApp.GetUserDetails().subscribe(
            (data) => {
                if (data.status === Status.Success) {
                    this.userModel = data.body;
                    this.croppedImage = AppConstants.keyString + data.body.profileImage;
                } else {
                    this.msgService.showError('Error');
                }
            }
        );
    }

    goBack() {
        this.router.navigate(['Dashboard']);
    }
}
