import { Component, OnInit } from '@angular/core';
import { isNullOrUndefined } from 'util';
import { Status } from '../../app.enum';

// Models
import { RoleViewModel } from '../../shared/customModels/role-view-model';
import { UserModel } from '../../shared/customModels/user-model';
import { ApprovalEventRoleViewModel } from '../../webapi/models/approval-event-role-view-model';

// Services
import { RoleServiceApp } from '../users/shared/role.serviceApp';
import { UserServiceApp } from '../users/shared/user.serviceApp';
import { DisplayMessageService } from '../../shared/toastr/display.message.service';
import { ApprovalServiceApp } from '../../approval/shared/approval.serviceApp';
import { TranslateService } from '@ngx-translate/core';
import { UserEventRoleServiceApp } from './shared/usereventrole.serviceApp';
import { NgForm } from '@angular/forms/src/directives/ng_form';
import { ApprovalModel } from '../../shared/customModels/approval-model';
import { ApprovalEventViewModel } from '../../shared/customModels/approval-event-view-model';
import { ApprovalService } from '../../webapi/services';

@Component({
    'selector': 'app-usereventrole',
    'templateUrl': 'usereventrole.component.html'
})

export class UserEventRoleComponent implements OnInit {

    approvalEvents: ApprovalEventViewModel[] = [] as ApprovalEventViewModel[];
    approvalEventRoleModel: ApprovalEventRoleViewModel = {} as ApprovalEventRoleViewModel;
    approvalRole: ApprovalService.ApiApprovalGetApprovedUsersByRoleGetParams = {} as
        ApprovalService.ApiApprovalGetApprovedUsersByRoleGetParams;
    approvalEventandTransaction: ApprovalService.ApiApprovalGetApprovalEventsGetParams = {} as
        ApprovalService.ApiApprovalGetApprovalEventsGetParams;
    roles: RoleViewModel[] = [] as RoleViewModel[];
    approvalEventRoleModels: ApprovalEventRoleViewModel[] = [] as ApprovalEventRoleViewModel[];
    users: UserModel[] = [];
    approvals: ApprovalModel[] = [];
    approvalId: number;
    defaultOption: any;

    constructor(private approvalServiceApp: ApprovalServiceApp, private msgService: DisplayMessageService,
        private roleServiceApp: RoleServiceApp, private userServiceApp: UserServiceApp,
        private translateService: TranslateService, private userEvetRoleServiceApp: UserEventRoleServiceApp) {

    }

    ngOnInit() {
        this.setDefaultOption();
        this.initializeMethods();
        this.getAllEventRoleModels();
        this.getUsers();
    }

    initializeMethods() {
        this.approvalEventRoleModel.users = [];
        this.getAllApprovals();
        this.getRoles();
    }

    getAllApprovals() {
        this.approvalServiceApp.getApprovals().subscribe(
            (data) => {
                if (data.status === Status.Success) {
                    this.approvals = data.body;
                    const approval = this.defaultOption;
                    this.approvals.splice(0, 0, { approvalId: 0, approvalName: approval });
                } else {
                    this.msgService.showError('Error');
                }
            }
        );
    }

    getAllEventRoleModels() {
        this.userEvetRoleServiceApp.getAllUserEventRole().subscribe(
            (data) => {
                if (data.status === Status.Success) {
                    this.approvalEventRoleModels = data.body;
                } else {
                    this.msgService.showError('Error');
                }
            }
        );
    }

    setDefaultOption() {
        this.translateService.get('COMMON.SELECTDEFAULT').subscribe(
            (data) => {
                this.defaultOption = data;
                this.getApprovalEvents();
            }
        );
    }

    onChangeUsers(user: UserModel, isChecked: boolean) {
        if (isChecked) {
            this.approvalEventRoleModel.users.push(user);
        } else {
            const index = this.approvalEventRoleModel.users.findIndex(x => x.userId === user.userId);
            if (index > -1) { this.approvalEventRoleModel.users.splice(index, 1); }
        }
    }

    getApprovalEvents() {
        this.approvalEventandTransaction.approvalId = this.approvalId;
        this.approvalEventandTransaction.entityId = null;
        if (!isNullOrUndefined(this.approvalId) && (String(this.approvalId) !== '0')) {
            this.approvalServiceApp.getApprovalEventsById(this.approvalEventandTransaction).subscribe(
                (data) => {
                    if (data.status === Status.Success) {
                        this.approvalEvents = data.body.approvalEventViewModel;
                        const approvalEvent = this.defaultOption;
                        this.approvalEvents.splice(0, 0, { approvalEventId: 0, approvalEventName: approvalEvent });
                        this.users = [];
                        this.getRoles();
                    } else {
                        this.msgService.showError('Error');
                    }
                }
            );
        } else {
            this.approvalEvents = [];
            const approvalEvent = this.defaultOption;
            this.approvalEvents.splice(0, 0, { approvalEventId: 0, approvalEventName: approvalEvent });
        }
    }

    setRoles() {
        this.getRoles();
        this.users = [];
    }

    getRoles() {
        this.roleServiceApp.getAllRoles().subscribe(
            (data) => {
                if (data.status === Status.Success) {
                    this.roles = data.body;
                    const role = this.defaultOption;
                    this.roles.splice(0, 0, { roleId: 0, name: role });
                } else {
                    this.msgService.showError('Error');
                }
            }
        );
    }

    getUsers() {
        if (!isNullOrUndefined(this.approvalEventRoleModel.roleId)) {
            this.userServiceApp.getUsersByRole(this.approvalEventRoleModel.roleId).subscribe(
                (data) => {
                    if (data.status === Status.Success) {
                        this.users = data.body;
                        this.approvalRole.approvalEventId = this.approvalEventRoleModel.approvalEventId;
                        this.approvalRole.roleId = this.approvalEventRoleModel.roleId;
                        this.userEvetRoleServiceApp.getApprovedUsers(this.approvalRole).subscribe(
                            (userData) => {
                                this.approvalEventRoleModel.users = userData.body;
                                this.checkedUsers(userData.body);
                            });
                    } else {
                        this.msgService.showError('Error');
                    }
                }
            );
        }
    }

    checkedUsers(userlist: UserModel[]) {
        this.users.forEach(x => x.isChecked = false);
        userlist.forEach((x) => {
            const user = this.users.filter(y => y.userId === x.userId);
            user.forEach((z) => z.isChecked = true);
        });
    }

    onSubmit(eventRoleForm) {
        if (eventRoleForm.valid) {
            if (this.approvalEventRoleModel.users.length > 0) {
                this.userEvetRoleServiceApp.CreateUserEventRole(this.approvalEventRoleModel).subscribe(
                    (data) => {
                        if (data.status === Status.Success) {
                            this.getAllEventRoleModels();
                        } else {
                            this.msgService.showError('Error');
                        }
                        this.reset();
                    }
                );
            } else {
                this.msgService.showWarning('EVENTROLE.MANDATORY');
            }
        }
    }

    reset() {
        this.users = [];
        this.approvalEvents = [];
        this.initializeMethods();
    }

}
