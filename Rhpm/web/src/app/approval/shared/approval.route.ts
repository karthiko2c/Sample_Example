import { Routes, RouterModule } from '@angular/router';
import { OpeningComponent } from '../../opening/opening.component';
import { CandidateComponent, CandidatesComponent, CandidateDetailsComponent } from '../../candidate/index.candidate';
// Components

// Role guard
import { RoleGuardService } from '../../shared/index.shared';


const Approval_ROUTES: Routes = [
];

export let ApprovalRouterModule = RouterModule.forRoot(Approval_ROUTES, { useHash: false });
