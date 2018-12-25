import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule, TimepickerModule  } from 'ngx-bootstrap';
import { DatePipe } from '@angular/common';

// Modules
import { CandidateRouterModule } from './candidate.route';

// Component
import {
    CandidateComponent, CandidatesComponent, AssignedUserComponent, CandidateDetailsComponent,
    ScheduleInterviewComponent
} from '../index.candidate';

// Services
import { CandidateServiceApp } from './candidate.serviceApp';
import { ApprovalModule } from '../../approval/shared/approval.module';

@NgModule({
    imports: [
        BrowserAnimationsModule,
        FormsModule,
        TranslateModule,
        CandidateRouterModule,
        ApprovalModule,
        BsDatepickerModule.forRoot(),
        TimepickerModule.forRoot()
    ],
    exports: [],
    declarations: [CandidateComponent, CandidatesComponent, AssignedUserComponent, CandidateDetailsComponent,
        ScheduleInterviewComponent],
    providers: [CandidateServiceApp, DatePipe],
})

export class CandidateModule {
}
