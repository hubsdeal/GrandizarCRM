import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LeadSalesTeamsComponent } from './leadSalesTeams.component';

const routes: Routes = [
    {
        path: '',
        component: LeadSalesTeamsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LeadSalesTeamRoutingModule {}
