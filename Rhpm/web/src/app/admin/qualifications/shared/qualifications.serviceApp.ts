import { Injectable } from '@angular/core';
import { Headers, Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import { QualificatonService } from '../../../webapi/services/qualificaton.service';

@Injectable()
export class QualificationsServiceApp {

    constructor(private apiQualificationsService: QualificatonService) { }
    addQualification(QualificationsModel): Observable<any> {
        return this.apiQualificationsService.ApiQualificatonCreateQualificationPost(QualificationsModel).map(x => (x));
    }

    getAllQualification(): Observable<any> {
        return this.apiQualificationsService.ApiQualificatonGetAllQualificationGet().map(x => (x));
    }

    deleteQualification(qualificationsModel): Observable<any> {
        return this.apiQualificationsService.ApiQualificatonDeleteQualificationPut(qualificationsModel).map(x =>(x));
    }

    updateQualification(QualificationsModel): Observable<any> {
        return this.apiQualificationsService.ApiQualificatonUpdateQualificationPut(QualificationsModel).map(x => (x));
    }

    getQualificationById(qualificatonId): Observable<any> {
        return this.apiQualificationsService.ApiQualificatonGetQualificationlByIdGet(qualificatonId).map(x => (x));
    }
    GetQualificationsResults(searchAndSortModel): Observable<any> {
        return this.apiQualificationsService.ApiQualificatonGetQualificationsResultsPost(searchAndSortModel).map(x => (x));
    }
}
