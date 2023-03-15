import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MembershipTypesComponent } from './membershipTypes.component';

const routes: Routes = [
    {
        path: '',
        component: MembershipTypesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MembershipTypeRoutingModule {}
