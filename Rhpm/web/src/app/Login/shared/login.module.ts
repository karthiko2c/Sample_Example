import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { BrowserModule } from '@angular/platform-browser';
import { SharedModule } from '../../shared/shared.module';

// Service
import { LoginServiceApp } from './login.serviceApp';
//import { ForgotpasswordServiceApp } from './forgotpassword.serviceApp';
// Route
import { LoginRouterModule } from './login.route';

// Components
import {
  LoginComponent, ForgotpasswordComponent
} from '../index.login';


@NgModule({
  imports: [
    BrowserModule, FormsModule,
    TranslateModule.forRoot(),
    LoginRouterModule
  ],
  exports: [
  ],
  declarations: [
    LoginComponent, ForgotpasswordComponent
  ],
  providers: [LoginServiceApp],
})

export class LoginModule {
}
