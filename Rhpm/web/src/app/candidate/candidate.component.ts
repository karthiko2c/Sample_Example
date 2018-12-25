import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { isNullOrUndefined } from 'util';

// Services
import { CandidateServiceApp } from './shared/candidate.serviceApp';
import { OpeningServiceApp } from '../opening/shared/opening.serviceApp';
import { QualificationsServiceApp } from '../admin/qualifications/shared/qualifications.serviceApp';
import { UtilityService } from '../shared/utility/utility.service';
import { TranslateService } from '@ngx-translate/core';

// Models
import { CandidateViewModel, QualificationViewModel, OpeningViewModel } from '../webapi/models';
import { Status, ApprovalType } from '../app.enum';
import { DisplayMessageService } from '../shared/toastr/display.message.service';
import { AppConstants } from '../shared/constant/constant.variable';
import { ApprovalResponseModel } from '../shared/customModels/approvel-response.model';
import { OrganizationViewModel } from '../shared/customModels/organization-view-model';

@Component({
    selector: 'app-candidate',
    templateUrl: 'candidate.component.html'
})

export class CandidateComponent implements OnInit {
    candidateModel: CandidateViewModel = {} as CandidateViewModel;
    openings: OpeningViewModel[] = [] as OpeningViewModel[];
    qualifications: QualificationViewModel[] = [] as QualificationViewModel[];
    organizations: OrganizationViewModel[] = [] as OrganizationViewModel[];
    organizationName: any;
    years: number[] = [];
    months: number[] = [];
    defaultOption: any;
    approval: any;
    uploadedFile: any;
    filePath: any;
    submitted = false;
    isUploaded = false;
    flag = true;
    config2: any = { 'placeholder': 'Enter Organization' };

    constructor(private openingServiceApp: OpeningServiceApp, private candidateServiceApp: CandidateServiceApp,
        private qualificationServiceApp: QualificationsServiceApp, private route: ActivatedRoute,
        private router: Router, private utilityService: UtilityService,
        private msgService: DisplayMessageService, private translateService: TranslateService) {
        this.years = this.utilityService.fillArray(30);
        this.months = this.utilityService.fillArray(12);
    }

    ngOnInit() {
        this.approval = ApprovalType.Candidate;
        this.setDefaultValues();
        this.initializeMethods();
        this.setDefaultOption();
    }

    setDefaultValues() {
        this.candidateModel.gender = AppConstants.defaultValue;
        this.candidateModel.experienceYear = AppConstants.defaultValue;
        this.candidateModel.experienceMonth = AppConstants.defaultValue;
    }

    initializeMethods() {
        this.getOpenings();
        this.getAllQualifications();
    }

    onInputChangedEvent(input: string) {
        this.flag = true;
        if (!isNullOrUndefined(input) && input.replace(/\s/g, '').length && input.length > AppConstants.minimumLength) {
            this.candidateServiceApp.getOrganizationOnInputChangedEvent(input).debounceTime(AppConstants.delayTime).subscribe(
                (data) => {
                    if (data.status === Status.Success) {
                        this.organizations = data.body;
                    } else {
                        this.msgService.showError('Error');
                    }
                }
            );
        } else {
            this.organizations = [];
        }
    }

    onSelectOrganization(organization) {
        if (!isNullOrUndefined(organization)) {
            this.candidateModel.organizationId = organization.organizationId;
            this.candidateModel.organizationName = organization.name;
            this.organizationName = organization.name;
            this.flag = false;
        } else {
            return false;
        }
    }

    getAllQualifications(): void {
        this.qualificationServiceApp.getAllQualification().subscribe(
            (data) => {
                if (data.status === Status.Success) {
                    this.qualifications = data.body;
                    const qualification = this.defaultOption;
                    this.qualifications.splice(0, 0, { qualificationId: 0, name: qualification });
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
            }
        );
    }

    getCandidateById() {
        this.route.params.subscribe((params: Params) => {
            const candidateId = params['candidateId'];
            if (!isNullOrUndefined(candidateId)) {
                this.candidateServiceApp.getCandidateById(candidateId).subscribe(
                    (data) => {
                        if (data.status === Status.Success) {
                            this.candidateModel = data.body;
                        } else {
                            this.msgService.showError('Error');
                        }
                    }
                );
            }
        });
    }

    getOpenings() {
        this.route.params.subscribe((params: Params) => {
            const openingId = params['openingId'];
            if (!isNullOrUndefined(openingId)) {
                this.openingServiceApp.getOpeningById(openingId).subscribe(
                    (data) => {
                        if (data.status === Status.Success) {
                            this.candidateModel.openingId = data.body.openingId;
                            this.openings.push(data.body);
                        } else {
                            this.msgService.showError('Error');
                        }
                    }
                );
            } else {
                this.openingServiceApp.getAllOpenings().subscribe(
                    (data) => {
                        if (data.status === Status.Success) {
                            this.openings = data.body;
                            const opening = this.defaultOption;
                            this.openings.splice(0, 0, { openingId: '0', title: opening });
                        } else {
                            this.msgService.showError('Error');
                        }
                        this.getCandidateById();
                    }
                );
            }
        });
    }

    showCandidateList() {
        this.router.navigate(['Candidates']);
    }

    onSubmit(candidateForm) {
        this.submitted = true;
        if (candidateForm.valid) {
            if (isNullOrUndefined(this.candidateModel.candidateId)) {
                this.candidateModel.organizationId = this.candidateModel.organizationName === this.organizationName ?
                    this.candidateModel.organizationId : 0;
                this.candidateServiceApp.addCandidate(AppConstants.uriForAddCandidate, this.candidateModel, this.uploadedFile).
                    subscribe(
                    (data) => {
                        if (data.body.status === Status.Success) {
                            this.showCandidateList();
                        } else {
                            this.msgService.showError('Error');
                        }
                    });
            } else {
                this.candidateServiceApp.updateCandidate(AppConstants.uriForUpdateCandidate, this.candidateModel, this.uploadedFile)
                    .subscribe(
                    (data) => {
                        if (data.body.status === Status.Success) {
                            this.showCandidateList();
                        } else {
                            this.msgService.showError('Error');
                        }
                    });
            }
        }
    }

    downloadCandidateResume(documentName, fileName) {
        this.candidateServiceApp.downloadCandiadteResume(AppConstants.uriForFile, documentName).subscribe(
            (data) => {
                debugger;
                const blobURL = window.URL.createObjectURL(data);
                const anchor = document.createElement('a');
                anchor.download = fileName;
                anchor.href = blobURL;
                anchor.click();
            }
        );
    }

    fileChange(event) {
        const fileList: FileList = event.target.files;
        if (fileList.length > 0) {
            this.isUploaded = true;
            const file: File = fileList[0];
            this.uploadedFile = file;
        }
    }
    assignUser() {
        if (!isNullOrUndefined(this.candidateModel.candidateId)) {
            this.router.navigate(['AssignedUser', this.candidateModel.candidateId]);
        }
    }

    goBack() {
        this.router.navigate(['Candidates']);
    }

}
