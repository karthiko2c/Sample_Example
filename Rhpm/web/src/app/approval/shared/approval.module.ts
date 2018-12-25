import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { BrowserModule } from '@angular/platform-browser';

// Route
import { ApprovalRouterModule } from './approval.route';

// Service
import { ApprovalServiceApp } from './approval.serviceApp';

// Components
import { StripComponent, ApprovalhistoryComponent } from '../index.approval';

// Module
import { SharedModule } from '../../shared/shared.module';


@NgModule({
  imports: [
    FormsModule,
    TranslateModule,
    BrowserModule,
    SharedModule,
    ApprovalRouterModule
  ],
  exports: [StripComponent, ApprovalhistoryComponent],
  declarations: [StripComponent, ApprovalhistoryComponent],
  providers: [ApprovalServiceApp],
})

export class ApprovalModule {
}
