import { Injectable } from '@angular/core';
import { Headers, Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import { SkillService } from '../../../webapi/services/skill.service';

@Injectable()
export class SkillsServiceApp {

    constructor(private apiSkillsService: SkillService) { }
    addSkill(SkillsModel): Observable<any> {
        return this.apiSkillsService.ApiSkillCreateSkillPost(SkillsModel).map(x => (x));
    }
    listSkill(): Observable<any> {
        return this.apiSkillsService.ApiSkillGetAllSkillGet().map(x => (x));
    }
    deleteSkill(skillsModel): Observable<any> {
        return this.apiSkillsService.ApiSkillDeleteSkillPut(skillsModel).map(x =>(x));
    }
    updateSkill(SkillsModel): Observable<any> {
        return this.apiSkillsService.ApiSkillUpdateSkillPut(SkillsModel).map(x => (x));
    }
    getSkillById(skillId): Observable<any> {
        return this.apiSkillsService.ApiSkillGetSkillByIdGet(skillId).map(x => (x));
    }
    GetSkillsResults(searchAndSortModel): Observable<any> {
        return this.apiSkillsService.ApiSkillGetSkillsResultsPost(searchAndSortModel).map(x => (x));
    }
}
