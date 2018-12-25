import { Injectable } from '@angular/core';
import { Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { map } from 'rxjs/operators';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import 'rxjs/Rx';

// Service
import { CandidateService } from '../../webapi/services/candidate.service';
import { UtilityService } from '../../shared/utility/utility.service';
import { ApiConfiguration } from '../../webapi/api-configuration';

@Injectable()
export class CandidateServiceApp {
    constructor(private apiCandidateSevice: CandidateService, private utilityService: UtilityService,
        private http: HttpClient, private apiConfig: ApiConfiguration) {
    }


    getAllCandidates(): Observable<any> {
        return this.apiCandidateSevice.ApiCandidateGetAllCandidateGet().map(x => (x));
    }

    getCandidateById(candidateId): Observable<any> {
        return this.apiCandidateSevice.ApiCandidateGetCandidateByIdGet(candidateId).map(x => (x));
    }

    addUserForCandidate(candidateAssignedUsers): Observable<any> {
        return this.apiCandidateSevice.ApiCandidateAddUserForCandidatePost(candidateAssignedUsers).map(x => (x));
    }

    scheduleUserForCandidateInterview(candidateAssignedUsers): Observable<any> {
        return this.apiCandidateSevice.ApiCandidateAddUsersToConductInterviewPost(candidateAssignedUsers).map(x => (x));
    }

    getAssignedUsersById(candidateId): Observable<any> {
        return this.apiCandidateSevice.ApiCandidateGetAssignedUsersByIdGet(candidateId).map(x => (x));
    }

    getScheduledUsersByCandidateId(candidateId): Observable<any> {
        return this.apiCandidateSevice.ApiCandidateGetScheduledUsersByIdGet(candidateId).map(x => (x));
    }

    getCandidatesCorrespondingToLoggedUser(userId): Observable<any> {
        return this.apiCandidateSevice.ApiCandidateGetCandidatesCorrespondingToLoggedUserGet(userId).map(x => (x));
    }

    approvedForInterview(candidateId): Observable<any> {
        return this.apiCandidateSevice.ApiCandidateApprovedForInterviewPut(candidateId);
    }

    getOrganizationOnInputChangedEvent(input): Observable<any> {
        return this.apiCandidateSevice.ApiCandidateGetOrganizationsOnInputChangedGet(input).map(x => (x));
    }

    addCandidate(uri, candidate, fileToUpload: any): Observable<any> {
        const url = this.apiConfig.rootUrl + uri;
        const formdata = new FormData();
        formdata.append('candidate', JSON.stringify(candidate));
        formdata.append('uploadFile', fileToUpload);
        return this.http.post(url, formdata, { observe: 'response' }).map(x => (x));
    }

    updateCandidate(uri, candidate, fileToUpload: any): Observable<any> {
        const url = this.apiConfig.rootUrl + uri;
        const formdata = new FormData();
        formdata.append('candidate', JSON.stringify(candidate));
        formdata.append('uploadFile', fileToUpload);
        return this.http.put(url, formdata, { observe: 'response' }).map(x => (x));
    }

    downloadCandiadteResume(uri, fileName): Observable<any> {
        const url = this.apiConfig.rootUrl + uri;
        const formdata = new FormData();
        formdata.append('file', fileName);
        return this.http.post(url, formdata, { responseType: 'blob' }).map(x => (x));
    }
}
