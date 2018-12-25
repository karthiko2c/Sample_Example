import { Component, OnInit } from '@angular/core';
import { OpeningServiceApp } from './shared/opening.serviceApp';
import { Router } from '@angular/router';
import { Status } from '../app.enum';
import { DisplayMessageService } from '../shared/toastr/display.message.service';
import { AppConstants } from '../shared/constant/constant.variable';
import { OpeningViewModel } from '../webapi/models';
import decode from 'jwt-decode';

@Component({
    selector: 'app-openings',
    templateUrl: 'openings.component.html'
})

export class OpeningsComponent implements OnInit {

    openings: OpeningViewModel[] = [] as OpeningViewModel[];
    loggedRole: any;
    userId: any;

    constructor(private openingServiceApp: OpeningServiceApp, private router: Router,
        private msgService: DisplayMessageService) {
    }

    ngOnInit() {
        this.setLoggedUser();
        this.getAllOpenings();
    }

    setLoggedUser() {
        let tokenPayload = '';
        const token = localStorage.getItem(AppConstants.AuthToken);
        // decode the token to get its payload
        if (token !== null) {
            tokenPayload = decode(token);
            this.loggedRole = tokenPayload[AppConstants.RoleClaim];
            this.userId = tokenPayload[AppConstants.IdClaim];
        }
    }

    getAllOpenings() {
        this.openingServiceApp.getOpeningsCorrespondingToLoggedUser(this.userId).subscribe(
            (data) => {
                if (data.status === Status.Success) {
                    this.openings = data.body;
                } else {
                    this.msgService.showError('Error');
                }
            }
        );
    }
    addOpening() {
        this.router.navigate(['opening']);
    }

    updateOpening(openingId) {
        this.router.navigate(['opening', openingId]);
    }

    addCandidate(openingId) {
        this.router.navigate(['opening/Candidate', openingId]);
    }

    openingDetails(openingId) {
        this.router.navigate(['openings', openingId]);
    }
}
