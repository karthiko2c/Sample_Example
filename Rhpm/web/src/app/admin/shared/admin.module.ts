import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { BrowserModule } from '@angular/platform-browser';

// Route
import { AdminRouterModule } from './admin.route';

// Service
import { UserServiceApp } from '../users/shared/user.serviceApp';
import { QualificationsServiceApp } from '../qualifications/shared/qualifications.serviceApp';
import { SkillsServiceApp } from '../skills/shared/skills.serviceApp';
import { RoleServiceApp } from '../users/shared/role.serviceApp';
import { UserEventRoleServiceApp } from '../usereventroles/shared/usereventrole.serviceApp';
import { LoginServiceApp } from '../../Login/shared/login.serviceApp';

// Components
import {
  QualificationsComponent, SkillsComponent, UserComponent, UsersComponent, UserEventRoleComponent
} from '../index.admin';

// Module
import { MaterialModule } from '../../material.module';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  imports: [
    FormsModule,
    TranslateModule,
    BrowserModule,
    MaterialModule,
    SharedModule,
    AdminRouterModule
  ],
  exports: [],
  declarations: [
    QualificationsComponent, SkillsComponent, UserComponent, UsersComponent, UserEventRoleComponent
  ],
  bootstrap: [],
  providers: [LoginServiceApp, SkillsServiceApp, UserServiceApp,
    QualificationsServiceApp, RoleServiceApp, UserEventRoleServiceApp]
})

export class AdminModule {
}
