import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderTeamsComponent } from './orderTeams.component';

const routes: Routes = [
    {
        path: '',
        component: OrderTeamsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class OrderTeamRoutingModule {}
