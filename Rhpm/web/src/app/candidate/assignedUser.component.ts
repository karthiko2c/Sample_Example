import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Location } from '@angular/common';

import { ApprovalServiceApp } from '../approval/shared/approval.serviceApp';
import { ApprovalEventViewModel } from '../shared/customModels/approval-event-view-model';
import { ApprovalType, Status } from '../app.enum';
import { UserViewModel, CandidateAssignedUserModel } from '../webapi/models';
import { CandidateServiceApp } from './shared/candidate.serviceApp';
import { UserModel } from '../shared/customModels/user-model';
import { UserServiceApp } from '../admin/users/shared/user.serviceApp';
import { isNullOrUndefined } from 'util';
import { ApprovalService } from '../webapi/services';

@Component({
    selector: 'app-assigneduser',
    templateUrl: 'assignedUser.component.html'
})

export class AssignedUserComponent implements OnInit {

    approvalEvents: ApprovalEventViewModel[] = [] as ApprovalEventViewModel[];
    candidateAssignedUsers: CandidateAssignedUserModel[] = [] as CandidateAssignedUserModel[];
    candidateAssignedUser: CandidateAssignedUserModel = {} as CandidateAssignedUserModel;
    approvalEventandTransaction: ApprovalService.ApiApprovalGetApprovalEventsGetParams = {} as
    ApprovalService.ApiApprovalGetApprovalEventsGetParams;

    constructor(private approvalServiceApp: ApprovalServiceApp, private userServiceApp: UserServiceApp,
        private route: ActivatedRoute, private candidateServiceApp: CandidateServiceApp,
        private router: Router, private _location: Location) {
    }

    ngOnInit() {
        this.getAllApprovalEvents();
    }

    getAllApprovalEvents() {
        this.approvalEventandTransaction.approvalId = ApprovalType.Candidate;
        this.approvalEventandTransaction.entityId = null;
        this.approvalServiceApp.getApprovalEventsById(this.approvalEventandTransaction).subscribe(
            (data) => {
                if (data.status === Status.Success) {
                    this.approvalEvents = data.body.approvalEventViewModel;
                    this.getCandidateId();
                } else {

                }
            });
    }

    onChangeUsers(user: UserViewModel, approvalEvent: ApprovalEventViewModel, isChecked: boolean) {
        if (isChecked) {
            const assignedUser = new CandidateAssignedUserModel;
            assignedUser.candidateId = this.candidateAssignedUser.candidateId;
            assignedUser.userId = user.userId;
            assignedUser.approvalEventId = approvalEvent.approvalEventId;
            this.candidateAssignedUsers.push(assignedUser);
        } else {
            const index = this.candidateAssignedUsers.findIndex(x => x.userId === user.userId);
            if (index > -1) { this.candidateAssignedUsers.splice(index, 1); }
        }
    }

    getCandidateId() {
        this.route.params.subscribe((params: Params) => {
            this.candidateAssignedUser.candidateId = params['candidateId'];
            if (!isNullOrUndefined(this.candidateAssignedUser.candidateId)) {
                this.getAssignedUsersById(this.candidateAssignedUser.candidateId);
            }
        });
    }

    getAssignedUsersById(candidateId) {
        this.candidateServiceApp.getAssignedUsersById(this.candidateAssignedUser.candidateId).subscribe(
            (data) => {
                if (data.status === Status.Success) {
                    this.candidateAssignedUsers = data.body;
                    this.checkedUsers(this.candidateAssignedUsers);
                } else {

                }
            }
        );
    }

    checkedUsers(usersList: CandidateAssignedUserModel[]) {
        this.approvalEvents.forEach(x => x.users.forEach(y => y.isChecked = false));
        usersList.forEach(x => {
            this.approvalEvents.forEach(y => {
                const users = y.users.filter(z => z.userId === x.userId && y.approvalEventId === x.approvalEventId);
                users.forEach(t => t.isChecked = true);
            });
        });
    }

    showCandidateList() {
        this.router.navigate(['Candidates']);
    }

    goBack(candidateId) {
        this.router.navigate(['Candidate', candidateId]);
    }

    onSubmit(assignUserForm) {
        if (assignUserForm.valid) {
            if (this.candidateAssignedUsers.length > 0) {
                this.candidateServiceApp.addUserForCandidate(this.candidateAssignedUsers).subscribe(
                    (data) => {
                        if (data.status === Status.Success) {
                            this.showCandidateList();
                        } else {

                        }
                    }
                );
            }
        }
    }
}
