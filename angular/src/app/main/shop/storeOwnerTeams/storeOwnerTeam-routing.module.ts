import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreOwnerTeamsComponent } from './storeOwnerTeams.component';

const routes: Routes = [
    {
        path: '',
        component: StoreOwnerTeamsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreOwnerTeamRoutingModule {}
