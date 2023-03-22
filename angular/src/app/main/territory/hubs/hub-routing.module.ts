import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HubDashboardComponent } from './hub-dashboard/hub-dashboard.component';
import { HubsComponent } from './hubs.component';

const routes: Routes = [
    {
        path: '',
        component: HubsComponent,
        pathMatch: 'full',
    },
    {
        path: 'dashboard/:hubId',
        component: HubDashboardComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubRoutingModule {}
