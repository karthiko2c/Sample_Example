import { Component, OnInit } from '@angular/core';
import { ApprovalService } from '../webapi/services';
import { ApprovalType, Status } from '../app.enum';
import { ApprovalEventViewModel } from '../shared/customModels/approval-event-view-model';
import { ApprovalServiceApp } from '../approval/shared/approval.serviceApp';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { CandidateServiceApp } from './shared/candidate.serviceApp';
import { UserServiceApp } from '../admin/users/shared/user.serviceApp';
import { ScheduleUserForCandidateModel } from '../webapi/models/schedule-user-for-candidate-model';
import { isNullOrUndefined } from 'util';
import { UserViewModel } from '../webapi/models';
import { ScheduleInterviewModel } from '../shared/customModels/schedule-interview-model';
import { DatePipe } from '@angular/common';
import { DisplayMessageService } from '../shared/toastr/display.message.service';

@Component({
    selector: 'app-scheduleinterview',
    templateUrl: 'scheduleInterview.component.html'
})

export class ScheduleInterviewComponent implements OnInit {

    scheduleUserForCandidateList: ScheduleUserForCandidateModel[] = [] as ScheduleUserForCandidateModel[];
    scheduleInterviewModel: ScheduleInterviewModel = {} as ScheduleInterviewModel;
    approvalEvents: ApprovalEventViewModel[] = [] as ApprovalEventViewModel[];
    eventOrder: number;
    approvalEventId: number;
    date: Date;
    time: any;
    scheduledUsersList: any[] = [];
    candidateScheduledUser: ScheduleUserForCandidateModel = {} as ScheduleUserForCandidateModel;
    approvalEventandTransaction: ApprovalService.ApiApprovalGetApprovalEventsGetParams = {} as
        ApprovalService.ApiApprovalGetApprovalEventsGetParams;
    allowMouseWheel = true;
    isMeridian = false;
    minDate = new Date();

    constructor(private approvalServiceApp: ApprovalServiceApp, private userServiceApp: UserServiceApp,
        private candidateServiceApp: CandidateServiceApp, private route: ActivatedRoute,
        private router: Router, public datepipe: DatePipe, private msgService: DisplayMessageService) {
    }

    ngOnInit() {
        this.date = this.minDate;
        this.time = this.minDate;
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
                    this.msgService.showError('Error');
                }
            });
    }

    getCandidateId() {
        this.route.params.subscribe((params: Params) => {
            this.candidateScheduledUser.candidateId = params['candidateId'];
            if (!isNullOrUndefined(this.candidateScheduledUser.candidateId)) {
                this.getScheduledUsersByCandidateId(this.candidateScheduledUser.candidateId);
            }
        });
    }

    getScheduledUsersByCandidateId(candidateId) {
        this.candidateServiceApp.getScheduledUsersByCandidateId(this.candidateScheduledUser.candidateId).subscribe(
            (data) => {
                if (data.status === Status.Success) {
                    this.scheduleInterviewModel = data.body;
                    this.eventOrder = this.scheduleInterviewModel.finishedEventOrder + 1;
                    this.approvalEventId = this.scheduleInterviewModel.nextApprovalEvent;
                    this.scheduleUserForCandidateList = this.scheduleInterviewModel.scheduleUserForCandidateModelList;
                    this.date = new Date(this.scheduleInterviewModel.scheduledDate);
                    this.time = this.date;
                    this.setUsersCompletedInterview();
                    this.checkedUsers(this.scheduleUserForCandidateList);
                } else {
                    this.msgService.showError('Error');
                }
            }
        );
    }

    setUsersCompletedInterview() {
        for (let i = 0; i < this.approvalEvents.length; i++) {
            this.approvalEvents[i].userCompletedInterview = this.scheduleInterviewModel.users[i];
            this.approvalEvents[i].interviewConductedOn = this.scheduleInterviewModel.interviewScheduledTime[i];
        }
    }

    onChangeUsers(user: UserViewModel, approvalEvent: ApprovalEventViewModel, isChecked: boolean) {
        if (isChecked) {
            const scheduledUser = new ScheduleUserForCandidateModel;
            scheduledUser.candidateId = this.candidateScheduledUser.candidateId;
            scheduledUser.user = user;
            scheduledUser.approvalEvent = approvalEvent;
            scheduledUser.userId = user.userId;
            scheduledUser.approvalEventId = approvalEvent.approvalEventId;
            this.scheduledUsersList.push(scheduledUser);
        } else {
            const index = this.scheduleUserForCandidateList.findIndex(x => x.userId === user.userId);
            if (index > -1) { this.scheduleUserForCandidateList.splice(index, 1); }
        }
    }

    checkedUsers(scheduledUsersList: ScheduleUserForCandidateModel[]) {
        this.approvalEvents.forEach(x => x.users.forEach(y => y.isChecked = false));
        scheduledUsersList.forEach(x => {
            this.approvalEvents.forEach(y => {
                const users = y.users.filter(z => z.userId === x.userId && y.approvalEventId === x.approvalEventId);
                users.forEach(t => t.isChecked = true);
            });
        });
    }

    onSubmit(scheduleInterviewForm) {
        this.scheduleUserForCandidateList = this.scheduleUserForCandidateList.filter(x => x.approvalEventId === this.approvalEventId);
        this.scheduleUserForCandidateList = this.scheduleUserForCandidateList.concat(this.scheduledUsersList);
        this.addScheduledDateTime();
        if (scheduleInterviewForm.valid) {
            if (this.scheduleUserForCandidateList.length > 0) {
                this.candidateServiceApp.scheduleUserForCandidateInterview(this.scheduleUserForCandidateList).subscribe(
                    (data) => {
                        if (data.status === Status.Success) {
                            this.showCandidateList();
                        } else {
                            this.msgService.showError('Error');
                        }
                    }
                );
            }
        }
    }

    addScheduledDateTime() {
        this.date.setHours(this.time.getHours(), this.time.getMinutes());
        const dateTime = this.datepipe.transform(this.date, 'yyyy-MM-dd HH:mm');
        this.scheduleUserForCandidateList.forEach(x => x.scheduledOn = dateTime);
    }

    showCandidateList() {
        this.router.navigate(['Candidates']);
    }

    goBack(candidateId) {
        this.router.navigate(['Candidate', candidateId]);
    }

}
