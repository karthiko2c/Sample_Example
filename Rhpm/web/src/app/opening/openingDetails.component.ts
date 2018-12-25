import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { OpeningViewModel } from '../webapi/models/opening-view-model';
import { isNullOrUndefined } from 'util';
import { OpeningServiceApp } from './shared/opening.serviceApp';
import { Status } from '../app.enum';
import { DisplayMessageService } from '../shared/toastr/display.message.service';

@Component({
    selector: 'app-openingdetails',
    templateUrl: 'openingDetails.component.html'
})

export class OpeningDetailsComponent implements OnInit {

    openingModel: OpeningViewModel = {} as OpeningViewModel;

    constructor(private route: ActivatedRoute, private openingServiceApp: OpeningServiceApp,
        private msgService: DisplayMessageService) {

    }

    ngOnInit() {
        this.getOpeningById();
    }

    getOpeningById() {
        this.route.params.subscribe((params: Params) => {
            const openingId = params['openingId'];
            if (!isNullOrUndefined(openingId)) {
                this.openingServiceApp.getOpeningById(openingId).subscribe(
                    (data) => {
                        if (data.status === Status.Success) {
                            this.openingModel = data.body;
                        } else {
                            this.msgService.showError('Error');
                        }
                    }
                );
            }
        });
    }
}

