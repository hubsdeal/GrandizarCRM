import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessUsersComponent } from './businessUsers.component';

const routes: Routes = [
    {
        path: '',
        component: BusinessUsersComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BusinessUserRoutingModule {}
