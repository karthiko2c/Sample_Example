import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { isNullOrUndefined } from 'util';

// Services
import { DisplayMessageService } from '../shared/toastr/display.message.service';
import { OpeningService } from '../webapi/services/opening.service';
import { OpeningServiceApp } from './shared/opening.serviceApp';

// Models
import { SkillViewModel } from '../webapi/models/skill-view-model';
import { SkillModel } from '../shared/customModels/skill-model';
import { ApprovalResponseModel } from '../shared/customModels/approvel-response.model';

import { OpeningSkillType, Status, ApprovalType } from '../app.enum';
import { ApprovalhistoryComponent } from './../approval/approvalhistory.component';
import { ApprovalServiceApp } from '../approval/shared/approval.serviceApp';
import { ApprovalTransactionViewModel, OpeningViewModel } from '../webapi/models';

@Component({
    selector: 'app-opening',
    templateUrl: 'opening.component.html'
})

export class OpeningComponent implements OnInit {

    openingModel: OpeningViewModel = {} as OpeningViewModel;
    skills: SkillViewModel[] = [] as SkillViewModel[];
    skillModels: SkillModel[] = [] as SkillViewModel[];
    primarySkillModels: SkillModel[] = [] as SkillModel[];
    secondarySkillModels: SkillModel[] = [] as SkillModel[];
    approvalTransaction: ApprovalTransactionViewModel = {} as ApprovalTransactionViewModel;
    approval: any;
    opening: any;
    details: any;
    isDataAvailable = false;
    submitted = false;

    constructor(private openingServiceApp: OpeningServiceApp, private route: ActivatedRoute,
        private router: Router, private msgService: DisplayMessageService,
        private approvalServiceApp: ApprovalServiceApp) {
    }

    ngOnInit() {
        this.initializeArray();
        this.opening = this.openingModel;
        this.approval = ApprovalType.Opening;
        this.getAllSkill();
        this.getOpeningById();
    }

    initializeArray() {
        this.openingModel.primarySkillTypes = [];
        this.openingModel.secondarySkillTypes = [];
    }

    getAllSkill() {
        this.openingServiceApp.getAllSkills().subscribe(
            (data) => {
                if (data.status === Status.Success) {
                    this.skills = data.body;
                    this.skillModels = this.skills.map(x => {
                        return {
                            skillId: x.skillId, name: x.name,
                            description: x.description, openingSkillType: x.openingSkillType, isChecked: false
                        };
                    });
                    this.primarySkillModels = this.skillModels.map(x => Object.assign({}, x));
                    this.secondarySkillModels = this.skillModels.map(x => Object.assign({}, x));
                } else {
                    this.msgService.showError('Error');
                }
            }
        );
    }

    onChangePrimarySkills(skill: SkillViewModel, isChecked: boolean) {
        if (isChecked) {
            skill.openingSkillType = OpeningSkillType.Primary;
            this.openingModel.primarySkillTypes.push(skill);
        } else {
            skill.openingSkillType = null;
            const index = this.openingModel.primarySkillTypes.findIndex(x => x.skillId === skill.skillId);
            if (index > -1) { this.openingModel.primarySkillTypes.splice(index, 1); }
        }
    }

    onChangeSecondarySkills(skill: SkillViewModel, isChecked: boolean) {
        if (isChecked) {
            skill.openingSkillType = OpeningSkillType.Secondary;
            this.openingModel.secondarySkillTypes.push(skill);
        } else {
            skill.openingSkillType = null;
            const index = this.openingModel.secondarySkillTypes.findIndex(x => x.skillId === skill.skillId);
            if (index > -1) { this.openingModel.secondarySkillTypes.splice(index, 1); }
        }
    }

    getOpeningById() {
        this.route.params.subscribe((params: Params) => {
            const openingId = params['openingId'];
            if (!isNullOrUndefined(openingId)) {
                this.openingServiceApp.getOpeningById(openingId).subscribe(
                    (data) => {
                        if (data.status === Status.Success) {
                            this.openingModel = data.body;
                            this.opening = this.openingModel;
                            this.checkedPrimarySkills(this.openingModel.primarySkillTypes);
                            this.checkedSecondarySkills(this.openingModel.secondarySkillTypes);
                        } else {
                            this.msgService.showError('Error');
                        }
                        this.opening = this.openingModel;
                        this.isDataAvailable = true;
                    }
                );
            } else {
                this.isDataAvailable = true;
            }
        });
    }

    checkedPrimarySkills(skillList: SkillViewModel[]): void {
        skillList.forEach((x) => {
            const skill = this.primarySkillModels.filter(y => y.skillId === x.skillId);
            skill.forEach((z) => z.isChecked = true);
        });
    }

    checkedSecondarySkills(skillList: SkillViewModel[]): void {
        skillList.forEach((x) => {
            const skill = this.secondarySkillModels.filter(y => y.skillId === x.skillId);
            skill.forEach((z) => z.isChecked = true);
        });
    }

    showOpeningList() {
        this.router.navigate(['openings']);
    }

    onSubmit(submit) {
        this.submitted = submit;
    }
}
