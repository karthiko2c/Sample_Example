import { Routes, RouterModule } from '@angular/router';

// Component
import { ChangepasswordComponent } from './../../detail/changepassword.component';
import { RoleGuardService } from '../../shared/index.shared';
import { AuthService } from '../../shared/guards/auth.service';
import { EditprofileComponent } from '../editprofile.component';


const DETAIL_ROUTES: Routes = [
    {
        path: 'changepassword',
        component: ChangepasswordComponent,
        canActivate: [RoleGuardService]
    },
    {
        path: 'editprofile',
        component: EditprofileComponent,
        canActivate: [RoleGuardService]
    }
];

export const DetailRouterModule = RouterModule.forRoot(DETAIL_ROUTES, { useHash: false });
