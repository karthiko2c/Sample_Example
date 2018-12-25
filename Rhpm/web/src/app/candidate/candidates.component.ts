import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CandidateServiceApp } from './shared/candidate.serviceApp';
import { CandidateListModel } from '../shared/customModels/candidate-list-model';
import { DisplayMessageService } from '../shared/toastr/display.message.service';
import { Status } from '../app.enum';
import { AppConstants } from '../shared/constant/constant.variable';
import { CandidateViewModel } from '../webapi/models/candidate-view-model';
import { TranslateService } from '@ngx-translate/core';
import decode from 'jwt-decode';
import { ApprovalServiceApp } from '../approval/shared/approval.serviceApp';
import { window } from 'rxjs/operator/window';

@Component({
    selector: 'app-candidates',
    templateUrl: 'candidates.component.html'
})

export class CandidatesComponent implements OnInit {
    candidates: CandidateListModel[] = [] as CandidateListModel[];
    candidate: CandidateViewModel = {} as CandidateViewModel;
    loggedRole: string;
    userId: any;
    promptMsg: any;

    constructor(private candidateServiceApp: CandidateServiceApp, private router: Router,
        private msgService: DisplayMessageService, private translateService: TranslateService,
        private approvalServiceApp: ApprovalServiceApp) {
    }

    ngOnInit() {
        this.getLoggedRole();
        this.getAllCandidates();
        this.setPromptMsg();
    }

    getLoggedRole() {
        let tokenPayload = '';
        const token = localStorage.getItem(AppConstants.AuthToken);
        // decode the token to get its payload
        if (token !== null) {
            tokenPayload = decode(token);
            this.loggedRole = tokenPayload[AppConstants.RoleClaim];
            this.userId = tokenPayload[AppConstants.IdClaim];
        }
    }

    getAllCandidates() {
        if (this.loggedRole === 'Sr.HR') {
            this.candidateServiceApp.getAllCandidates().subscribe(
                (data) => {
                    if (data.status === Status.Success) {
                        this.candidates = data.body;
                    } else {
                        this.msgService.showError('Error');
                    }
                }
            );
        } else {
            this.candidateServiceApp.getCandidatesCorrespondingToLoggedUser(this.userId).subscribe(
                (data) => {
                    if (data.status === Status.Success) {
                        this.candidates = data.body;
                    } else {
                        this.msgService.showError('Error');
                    }
                }
            );
        }
    }

    setPromptMsg() {
        this.translateService.get('CANDIDATE.DISPLAYALERTMSG').subscribe(
            (data) => {
                this.promptMsg = data;
            }
        );
    }

    addCandidate() {
        this.router.navigate(['Candidate']);
    }

    updateCandidate(candidateId) {
        this.router.navigate(['Candidate', candidateId]);
    }

    getCandidateDetail(candidateId) {
        this.router.navigate(['CandidateDetails', candidateId]);
    }

    startInterview(candidate) {
        this.approvalServiceApp.checkForStartInterview(candidate).subscribe(
            (data) => {
                if (data.status === Status.Success) {
                    if (data.body) {
                        alert(this.promptMsg);
                    } else {
                        this.router.navigate(['CandidateDetails', candidate.candidateId]);
                    }
                } else {

                }
            }
        );
    }

    scheduleInterview(candidateId) {
        this.router.navigate(['ScheduleInterview', candidateId]);
    }

}
