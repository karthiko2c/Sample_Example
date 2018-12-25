import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { BrowserModule } from '@angular/platform-browser';

// Route
import { DashboardRouterModule } from './dashboard.route';

// Service


// Components
import {
  DashboardComponent, AdminDashboardComponent, HrDashboardComponent
} from '../index.dashboard';

// Module
import { SharedModule } from '../../shared/shared.module';


@NgModule({
  imports: [
    FormsModule,
    TranslateModule,
    BrowserModule,
    SharedModule,
    DashboardRouterModule,
  ],
  exports: [],
  declarations: [
    DashboardComponent, AdminDashboardComponent, HrDashboardComponent
  ],
  providers: [],
})

export class DashboardModule {
}
