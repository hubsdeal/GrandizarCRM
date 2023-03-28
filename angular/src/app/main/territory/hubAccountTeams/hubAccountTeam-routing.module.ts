import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HubAccountTeamsComponent } from './hubAccountTeams.component';

const routes: Routes = [
    {
        path: '',
        component: HubAccountTeamsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubAccountTeamRoutingModule {}
