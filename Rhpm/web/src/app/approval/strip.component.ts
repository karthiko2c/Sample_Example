import { Component, OnInit, Input, Output, EventEmitter, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import decode from 'jwt-decode';

import { ApprovalServiceApp } from './shared/approval.serviceApp';
import { AppConstants } from '../shared/constant/constant.variable';
import { isNullOrUndefined } from 'util';
import { ApprovalType, Status } from '../app.enum';
import { EntityAndApprovalViewModel, ApprovalTransactionViewModel, CandidateViewModel, OpeningViewModel } from '../webapi/models';
import { OpeningServiceApp } from '../opening/shared/opening.serviceApp';
import { ApprovalEventViewModel } from '../shared/customModels/approval-event-view-model';
import { DisplayMessageService } from '../shared/toastr/display.message.service';
import { OpeningModule } from '../opening/shared/opening.module';
import { ApprovalService, CandidateService } from '../webapi/services';
import { CandidateModule } from '../candidate/shared/candidate.module';
import { CandidateServiceApp } from '../candidate/shared/candidate.serviceApp';

@Component({
  selector: 'app-approvalstrip',
  templateUrl: 'strip.component.html',
  styleUrls: ['shared/strip.scss'],
})

export class StripComponent implements OnInit {

  approvalEvents: ApprovalEventViewModel[] = [] as ApprovalEventViewModel[];
  currentEventClicked: ApprovalEventViewModel = {} as ApprovalEventViewModel;
  entityAndApprovalEventModel: EntityAndApprovalViewModel = {} as EntityAndApprovalViewModel;
  approvalEventandTransaction: ApprovalService.ApiApprovalGetApprovalEventsGetParams = {} as
    ApprovalService.ApiApprovalGetApprovalEventsGetParams;

  approvalUserTypes: any;
  comments = null;
  clicked: boolean;
  showPopup: boolean;
  openingModel: OpeningViewModel = {} as OpeningViewModel;
  candidateModel: CandidateViewModel = [] as CandidateViewModel;
  approvalTransaction: ApprovalTransactionViewModel = {} as ApprovalTransactionViewModel;
  nextEventOrder: any;
  currentEventOrder: any;
  styleleft: any;
  permissibleEvent: any;
  submitted = false;
  userId: any;

  @ViewChild('source') source;
  @Input() approvalType: number;
  @Input() entityModel: any;
  @Output() onSubmit: EventEmitter<any> = new EventEmitter<any>();

  constructor(private router: Router, private approvalServiceApp: ApprovalServiceApp,
    private openingServiceApp: OpeningServiceApp, private msgService: DisplayMessageService,
    private eleRef: ElementRef, private _renderer: Renderer2, private candidateServiceApp: CandidateServiceApp) {

  }

  ngOnInit() {
    this.approvalEventandTransaction.entityId = null;
    this.clicked = true;
    this.onSubmit.emit(false);
    this.showPopup = false;
    this.currentEventClicked = null;
    this.approvalUserTypes = null;
    this.getLoggedUser();

    if (this.approvalType === ApprovalType.Opening) {
      if (!isNullOrUndefined(this.entityModel)) {
        this.openingModel = this.entityModel as OpeningViewModel;
        this.entityAndApprovalEventModel.openingViewModel = this.openingModel;
        this.entityAndApprovalEventModel.candidateViewModel = null;
      }
    } else {
      if (!isNullOrUndefined(this.entityModel)) {
        this.candidateModel = this.entityModel as CandidateViewModel;
        this.entityAndApprovalEventModel.candidateViewModel = this.candidateModel;
        this.entityAndApprovalEventModel.openingViewModel = null;
      }
    }

    this.getAllApprovalEvents();
    this.getUserApprovalRole();
  }

  getLoggedUser() {
    let tokenPayload = '';
    const token = localStorage.getItem(AppConstants.AuthToken);
    if (!isNullOrUndefined(token)) { tokenPayload = decode(token); }
    this.userId = tokenPayload[AppConstants.IdClaim];
  }

  getAllApprovalEvents() {
    this.approvalEventandTransaction.entityId = !isNullOrUndefined(this.openingModel.openingId) ?
      this.openingModel.openingId : this.candidateModel.candidateId;
    this.approvalEventandTransaction.approvalId = this.approvalType;
    this.approvalServiceApp.getApprovalEventsById(this.approvalEventandTransaction).subscribe((data) => {
      this.approvalEvents = data.body.approvalEventViewModel;
      if (data.body.approvalTransactionViewModel !== null) {
        this.approvalTransaction = data.body.approvalTransactionViewModel;
        this.nextEventOrder = this.approvalTransaction.nextEventOrderNumber;
        this.currentEventOrder = this.approvalTransaction.eventOrderNumber;
        this.permissibleEvent = this.approvalTransaction.permissibleEvent;
      } else {
        this.nextEventOrder = 1;
        this.permissibleEvent = 1;
      }
    });
  }

  getUserApprovalRole() {
    if (this.approvalType === ApprovalType.Opening) {
      let tokenPayload = '';
      const token = localStorage.getItem(AppConstants.AuthToken);
      if (!isNullOrUndefined(token)) { tokenPayload = decode(token); }
      this.approvalUserTypes = JSON.parse(tokenPayload[AppConstants.ApprovalTypeRole]);
    } else {
      this.approvalServiceApp.getApprovalEventsOfUserForCandidate(this.entityModel.candidateId).subscribe(
        (data) => {
          if (data.status === Status.Success) {
            this.approvalUserTypes = data.body;
          } else {
            this.msgService.showError('Error');
          }
        }
      );
    }

  }

  isDisabled(approvalEventId) {
    const approval = ApprovalType[this.approvalType];
    const userApprovalEventTypes = this.approvalUserTypes[approval];
    if (userApprovalEventTypes.find(t => t === approvalEventId)) {
      return false;
    }
    return true;
  }

  onApprovalEventClick(approvalEvent) {
    this.showPopup = false;
    this.currentEventClicked = approvalEvent;
    if (this.currentEventClicked.approvalEventOrder === this.nextEventOrder &&
      this.currentEventClicked.approvalEventOrder === this.permissibleEvent) {
      const ulElement = this.source.nativeElement;
      const itemIndex = this.approvalEvents.indexOf(approvalEvent);
      if (itemIndex !== -1 && itemIndex > 0) {
        let totalWidth = 0;
        for (let i = 0; i < itemIndex; i++) {
          totalWidth += ulElement.children[i].clientWidth;
        }
        this.styleleft = (totalWidth + 20) + 'px';
      } else {
        this.styleleft = '20px';
      }
      this.showPopup = true;
      this.clicked = true;
    }
  }

  onApprovalActionClick(approvalActions) {
    this.submitted = true;
    this.clicked = this.comments === null || this.comments === '' ? true : false;
    this.onSubmit.emit(true);
    this.approvalTransaction.eventOrderNumber = this.approvalTransaction.nextEventOrderNumber;
    this.approvalTransaction.approvalActionId = approvalActions.approvalActionId;
    if (this.currentEventClicked.approvalEventOrder === this.approvalTransaction.nextEventOrderNumber ||
      (isNullOrUndefined(this.approvalTransaction.nextEventOrderNumber)
      )) {
      const approvalEventOrders = this.approvalEvents.map(x => x.approvalEventOrder);
      if (this.currentEventClicked.approvalEventOrder === Math.min.apply(null, approvalEventOrders) &&
        this.approvalType === ApprovalType.Opening) {
        this.insertOrUpdateOpening();
      } else {
        const approvalActionIds = this.currentEventClicked.approvalActions.map(x => x.approvalActionId);
        this.approvalTransaction.comments = this.comments;
        if (approvalActions.approvalActionId === Math.min.apply(null, approvalActionIds)) {
          this.approvalTransaction.nextEventOrderNumber = this.approvalType === ApprovalType.Opening ?
            this.approvalTransaction.nextEventOrderNumber - 1 : -1;
        }
        if (approvalActions.approvalActionId === Math.max.apply(null, approvalActionIds) &&
          !isNullOrUndefined(this.approvalTransaction.nextEventOrderNumber)) {
          this.approvalTransaction.nextEventOrderNumber += 1;
        }
        this.insertApprovalTransaction();
        if (this.approvalTransaction.nextEventOrderNumber <= this.approvalEvents.length) {
          this.approvalServiceApp.manageApprovalTransaction(this.entityAndApprovalEventModel).subscribe(
            (data) => {
              if (data.status === Status.Success) {
                this.nextEventOrder = data.body.nextEventOrderNumber;
                this.currentEventOrder = data.body.eventOrderNumber;
                this.msgService.showInfo('COMMON.INFO');
              } else {
                this.msgService.showError('Error');
              }
            });
        } else {
          this.approvalTransaction.nextEventOrderNumber = 0;
          this.insertApprovalTransaction();
          this.approvalServiceApp.manageApprovalTransaction(this.entityAndApprovalEventModel).subscribe(
            (data) => {
              if (data.status === Status.Success) {
                this.nextEventOrder = data.body.nextEventOrderNumber;
                this.currentEventOrder = data.body.eventOrderNumber;
                this.msgService.showInfo('COMMON.INFO');
              } else {
                this.msgService.showError('Error');
              }
            });
        }
      }
    }
  }

  insertOrUpdateOpening() {
    if (this.isValidate(this.openingModel)) {
      if (isNullOrUndefined(this.openingModel.openingId)) {
        this.entityAndApprovalEventModel.approvalTransactionViewModel = this.currentEventClicked;
        this.entityAndApprovalEventModel.approvalTransactionViewModel.approvalActionId = this.approvalTransaction.approvalActionId;
      } else {
        this.entityAndApprovalEventModel.approvalTransactionViewModel = this.approvalTransaction;
      }
      this.entityAndApprovalEventModel.approvalTransactionViewModel.comments = this.comments;
      this.openingServiceApp.CreateOrUpdateOpening(this.entityAndApprovalEventModel).subscribe(
        (data) => {
          if (data.status === Status.Success) {
            this.nextEventOrder = data.body.nextEventOrderNumber;
            this.currentEventOrder = data.body.eventOrderNumber;
            this.msgService.showInfo('COMMON.INFO');
          } else {
            this.msgService.showError('Error');
          }
        });
    }
  }

  insertApprovalTransaction() {
    const approvalEventOrders = this.approvalEvents.map(x => x.approvalEventOrder);
    if (this.currentEventClicked.approvalEventOrder === Math.min.apply(null, approvalEventOrders)) {
      this.entityAndApprovalEventModel.approvalTransactionViewModel = this.currentEventClicked;
      this.entityAndApprovalEventModel.approvalTransactionViewModel.approvalActionId = this.approvalTransaction.approvalActionId;
    } else {
      this.entityAndApprovalEventModel.approvalTransactionViewModel = this.approvalTransaction;
    }
  }

  isValidate(opening: OpeningViewModel): boolean {
    if (isNullOrUndefined(opening.title) || isNullOrUndefined(opening.description)) {
      return false;
    } else if (opening.primarySkillTypes.length <= 0) {
      return false;
    } else if (this.SameSkillinBothSkillType(opening)) {
      this.msgService.showInfo('OPENING.SAMESKILLS');
      return false;
    }
    return true;
  }

  SameSkillinBothSkillType(opening): boolean {
    const sameSkill = [];
    opening.primarySkillTypes.forEach(skill => {
      opening.secondarySkillTypes.forEach(value => {
        if (skill.skillId === value.skillId) {
          sameSkill.push(skill);
        }
      });
    });
    if (sameSkill.length > 0) { return true; }
    return false;
  }

  close() {
    this.clicked = false;
    this.submitted = false;
  }

}
