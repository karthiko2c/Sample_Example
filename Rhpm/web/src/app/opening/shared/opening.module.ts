import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

// Modules
import { OpeningRouterModule } from './opening.route';
import { ApprovalModule } from '../../approval/shared/approval.module';

// Components
import { OpeningComponent, OpeningsComponent, OpeningDetailsComponent } from '../index.opening';

// Services
import { OpeningServiceApp } from './opening.serviceApp';


@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        TranslateModule,
        OpeningRouterModule, ApprovalModule
    ],
    exports: [],
    declarations: [
        OpeningsComponent,
        OpeningComponent,
        OpeningDetailsComponent
    ],
    providers: [OpeningServiceApp],
})

export class OpeningModule {
}
