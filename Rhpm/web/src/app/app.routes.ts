import { Routes, RouterModule } from '@angular/router';
import { ErrorComponent } from './error/error.component';
import { RoleGuardService } from './shared/index.shared';

const APP_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: '**',
    component: ErrorComponent
  },
];

export let AppRouterModule = RouterModule.forRoot(APP_ROUTES, { useHash: false });

