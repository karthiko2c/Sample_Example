import { Injectable } from '@angular/core';
import { Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { OpeningService, SkillService, ApprovalService } from '../../webapi/services';
import 'rxjs/Rx';
import { OpeningViewModel } from '../../webapi/models';

@Injectable()
export class OpeningServiceApp {

    constructor(private http: HttpClient,private apiApprovalService: ApprovalService, private apiOpeningService: OpeningService, private apiSkillService: SkillService) { }

    CreateOrUpdateOpening(entityAndApprovalEventModel): Observable<any> {
        return this.apiOpeningService.ApiOpeningInsertOrUpdateOpeningPost(entityAndApprovalEventModel).map(x => (x));
    }

    getAllSkills(): Observable<any> {
        return this.apiSkillService.ApiSkillGetAllSkillGet().map(x => (x));
    }

    getAllOpenings(): Observable<any> {
        return this.apiOpeningService.ApiOpeningGetAllOpeningGet().map(x => (x));
    }

    getOpeningsCorrespondingToLoggedUser(userId): Observable<any> {
        return this.apiOpeningService.ApiOpeningGetOpeningsCorrespondingToLoggedUserGet(userId).map(x => (x));
    }

    getOpeningById(openingId): Observable<any> {
        return this.apiOpeningService.ApiOpeningGetOpeningByIdGet(openingId).map(x => (x));
    }
}
