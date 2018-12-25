import { Component, OnInit, Input } from '@angular/core';
import { Router, Params, ActivatedRoute } from '@angular/router';
import { ApprovalServiceApp } from './shared/approval.serviceApp';
import { ApprovalResponseModel } from '../shared/customModels/approvel-response.model';
import { Status } from '../app.enum';
import { DisplayMessageService } from '../shared/toastr/display.message.service';

@Component({
    selector: 'app-approvalhistory',
    templateUrl: 'approvalhistory.component.html',
    styleUrls: ['./shared/strip.scss'],
})

export class ApprovalhistoryComponent implements OnInit {
    approvalResponseModels: ApprovalResponseModel[] = [] as ApprovalResponseModel[];

    @Input() entityId: any;

    constructor(private router: Router, private msgService: DisplayMessageService, private approvalServiceApp: ApprovalServiceApp,
        private route: ActivatedRoute) {
    }
    ngOnInit() {
        this.getApprovalDetails();
    }
    getApprovalDetails() {
        this.approvalServiceApp.getApprovalDetails(this.entityId).subscribe(
            (data) => {
                if (data.status === Status.Success) {
                    this.approvalResponseModels = data.body;
                } else {
                    this.msgService.showError('Error');
                }
            }
        );
    }
}
