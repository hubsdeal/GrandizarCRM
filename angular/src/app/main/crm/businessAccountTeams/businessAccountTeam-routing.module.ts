import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessAccountTeamsComponent } from './businessAccountTeams.component';

const routes: Routes = [
    {
        path: '',
        component: BusinessAccountTeamsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BusinessAccountTeamRoutingModule {}
