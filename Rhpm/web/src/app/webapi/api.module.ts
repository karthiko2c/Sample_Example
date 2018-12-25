import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { ApiConfiguration } from './api-configuration';

import { ApprovalService } from './services/approval.service';
import { CandidateService } from './services/candidate.service';
import { LoginService } from './services/login.service';
import { OpeningService } from './services/opening.service';
import { QualificatonService } from './services/qualificaton.service';
import { RoleService } from './services/role.service';
import { SkillService } from './services/skill.service';
import { UserService } from './services/user.service';

/**
 * Module that provides instances for all API services
 */
@NgModule({
  imports: [
    HttpClientModule
  ],
  exports: [
    HttpClientModule
  ],
  declarations: [],
  providers: [
    ApiConfiguration,
   ApprovalService,
   CandidateService,
   LoginService,
   OpeningService,
   QualificatonService,
   RoleService,
   SkillService,
   UserService
  ],
})
export class ApiModule { }
