import { Routes, RouterModule } from '@angular/router';

// Service
import { RoleGuardService } from '../../shared/index.shared';

// Component
import {
    CandidateComponent, CandidatesComponent, AssignedUserComponent, CandidateDetailsComponent,
    ScheduleInterviewComponent
} from '../index.candidate';

const CANDIDATE_ROUTES: Routes = [
    {
        path: 'Candidates',
        component: CandidatesComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: ['Sr.HR', 'Manager', 'VP'] }
    },
    {
        path: 'Candidate',
        component: CandidateComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: ['Sr.HR', 'Manager', 'VP'] }
    },
    {
        path: 'Candidate/:candidateId',
        component: CandidateComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: ['Sr.HR', 'Manager', 'VP'] }
    },
    {
        path: 'opening/Candidate/:openingId',
        component: CandidateComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: ['Sr.HR', 'Manager', 'VP'] }
    },
    {
        path: 'AssignedUser/:candidateId',
        component: AssignedUserComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: ['Sr.HR'] }
    },
    {
        path: 'CandidateDetails/:candidateId',
        component: CandidateDetailsComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: ['Sr.HR', 'Manager', 'VP'] }
    },
    {
        path: 'ScheduleInterview/:candidateId',
        component: ScheduleInterviewComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: ['Sr.HR'] }
    }
];

export const CandidateRouterModule = RouterModule.forRoot(CANDIDATE_ROUTES, { useHash: false });
