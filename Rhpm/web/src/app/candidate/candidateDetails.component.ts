import { Component, OnInit } from '@angular/core';
import { CandidateViewModel, ApprovalTransactionViewModel } from '../webapi/models';
import { Params, ActivatedRoute } from '@angular/router';
import { CandidateServiceApp } from './shared/candidate.serviceApp';
import { isNullOrUndefined } from 'util';
import { Status, ApprovalType } from '../app.enum';
import { DisplayMessageService } from '../shared/toastr/display.message.service';
import { QualificationsServiceApp } from '../admin/qualifications/shared/qualifications.serviceApp';
import { OpeningServiceApp } from '../opening/shared/opening.serviceApp';
import { ApprovalServiceApp } from '../approval/shared/approval.serviceApp';
import { TranslateService } from '@ngx-translate/core';
import { AppConstants } from '../shared/constant/constant.variable';
import decode from 'jwt-decode';

@Component({
    selector: 'app-candidatedetails',
    templateUrl: 'candidateDetails.component.html'
})

export class CandidateDetailsComponent implements OnInit {

    candidateModel: CandidateViewModel = {} as CandidateViewModel;
    approvalTransaction: ApprovalTransactionViewModel = {} as ApprovalTransactionViewModel;
    approval: any;
    opening: any;
    candidate: any;
    qualification: any;
    isDataAvailable = false;
    gender: any;
    loggedRole: any;

    constructor(private route: ActivatedRoute, private candidateServiceApp: CandidateServiceApp,
        private msgService: DisplayMessageService, private qualificationsServiceApp: QualificationsServiceApp,
        private openingServiceApp: OpeningServiceApp, private approvalServiceApp: ApprovalServiceApp,
        private translateService: TranslateService) { }

    ngOnInit() {
        this.candidate = this.candidateModel;
        this.approval = ApprovalType.Candidate;
        this.getCandidateById();
        this.getLoggedRole();
    }

    getLoggedRole() {
        let tokenPayload = '';
        const token = localStorage.getItem(AppConstants.AuthToken);
        // decode the token to get its payload
        if (token !== null) {
            tokenPayload = decode(token);
            this.loggedRole = tokenPayload[AppConstants.RoleClaim];
        }
    }

    getCandidateById() {
        this.route.params.subscribe((params: Params) => {
            const candidateId = params['candidateId'];
            if (!isNullOrUndefined(candidateId)) {
                this.candidateServiceApp.getCandidateById(candidateId).subscribe(
                    (data) => {
                        if (data.status === Status.Success) {
                            this.candidateModel = data.body;
                            this.candidate = this.candidateModel;
                            this.getGender(this.candidateModel.gender);
                        } else {
                            this.msgService.showError('Error');
                        }
                        this.isDataAvailable = true;
                    }
                );
            }
        });
    }

    getGender(id) {
        const genderName = id === 1 ? 'CANDIDATE.MALE' : 'CANDIDATE.FEMALE';
        this.translateService.get(genderName).subscribe(
            (data) => {
                this.gender = data;
            }
        );
    }

    downloadCandidateResume(documentName, fileName) {
        this.candidateServiceApp.downloadCandiadteResume(AppConstants.uriForFile, documentName).subscribe(
            (data) => {
                const blobURL = window.URL.createObjectURL(data);
                const anchor = document.createElement('a');
                anchor.download = fileName;
                anchor.href = blobURL;
                anchor.click();
            }
        );
    }
}
