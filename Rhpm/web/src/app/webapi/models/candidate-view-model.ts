/* tslint:disable */
import { CandidateDocumentViewModel } from './candidate-document-view-model';

/**
 */
export class CandidateViewModel {
    organizationName?: string;
    candidateId?: string;
    firstName?: string;
    lastName?: string;
    experienceYear?: number;
    experienceMonth?: number;
    description?: string;
    organizationId?: number;
    gender?: number;
    qualification?: number;
    qualificationName?: string;
    openingId?: string;
    openingTitle?: string;
    isApproved?: boolean;
    isReadyForInterview?: boolean;
    candidateDocument?: CandidateDocumentViewModel;
}
