import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { MomentModule } from 'angular2-moment';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';

// components
import { AppComponent } from './app.component';
import { AppRouterModule } from './app.routes';

// Module
import { LoginModule } from './Login/shared/login.module';
import { AdminModule } from './admin/shared/admin.module';
import { SharedModule } from './shared/shared.module';
import { ErrorModule } from './error/error.module';
import { OpeningModule } from './opening/shared/opening.module';
import { DashboardModule } from './dashboard/shared/dashboard.module';
import { CandidateModule } from './candidate/shared/candidate.module';


import { ApiModule } from './webapi/api.module';
import { DetailModule } from './detail/shared/detail.module';

// constants
import { AppConstants } from './shared/constant/constant.variable';

import {
  AuthService, RoleGuardService, SpinnerDirective, SpinnerService,
  AuthInterceptor, UtilityService, SharedService
} from './shared/index.shared';


export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

export function GetToken() {
  return localStorage.getItem(AppConstants.AuthToken);
}

import { ToasterModule, ToasterContainerComponent } from 'angular5-toaster';
import { DisplayMessageService } from './shared/toastr/display.message.service';
import { NgModel } from '@angular/forms/src/directives/ng_model';

@NgModule({
  declarations: [
    AppComponent, SpinnerDirective,
  ],
  imports: [
    AppRouterModule, ErrorModule, BrowserModule, FormsModule,
    MomentModule, HttpClientModule, ApiModule, LoginModule,
    AdminModule, SharedModule, OpeningModule, CandidateModule,
    DashboardModule, ToasterModule, BrowserAnimationsModule, DetailModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      }
    }),
    JwtModule.forRoot({
      config: {
        tokenGetter: GetToken,
        whitelistedDomains: [''] // example http://localhost:4200/
      }
    })
  ],
  exports: [],
  providers: [
    AuthService,
    RoleGuardService,
    JwtHelperService,
    SpinnerService,
    UtilityService,
    SharedService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    DisplayMessageService
  ],
  bootstrap: [AppComponent]
})

export class AppModule {
}
