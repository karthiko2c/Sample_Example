import { Routes, RouterModule } from '@angular/router';

// Components
import {
  DashboardComponent, HrDashboardComponent
} from '../index.dashboard';
import { RoleGuardService } from '../../shared/guards/role.guard.service';

const Dashboard_ROUTES: Routes = [
  {
    path: 'Dashboard',
    component: DashboardComponent
  }
];

export let DashboardRouterModule = RouterModule.forRoot(Dashboard_ROUTES, { useHash: false });
