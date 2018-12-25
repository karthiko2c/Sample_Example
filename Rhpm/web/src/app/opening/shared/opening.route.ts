import { Routes, RouterModule } from '@angular/router';

// Component

import { RoleGuardService } from '../../shared/index.shared';
import { OpeningComponent, OpeningsComponent, OpeningDetailsComponent } from '../index.opening';

const OPENING_ROUTES: Routes = [
    {
        path: 'opening',
        component: OpeningComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: ['Sr.HR', 'Manager', 'VP'] }
    },
    {
        path: 'opening/:openingId',
        component: OpeningComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: ['Sr.HR', 'Manager', 'VP'] }
    },
    {
        path: 'openings',
        component: OpeningsComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: ['Sr.HR', 'Manager', 'VP'] }
    },
    {
        path: 'openings/:openingId',
        component: OpeningDetailsComponent,
        canActivate: [RoleGuardService],
        data: { expectedRole: ['Sr.HR', 'Manager', 'VP'] }
    }
];

export const OpeningRouterModule = RouterModule.forRoot(OPENING_ROUTES, { useHash: false });
