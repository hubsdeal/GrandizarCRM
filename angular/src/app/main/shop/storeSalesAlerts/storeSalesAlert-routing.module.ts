import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreSalesAlertsComponent } from './storeSalesAlerts.component';

const routes: Routes = [
    {
        path: '',
        component: StoreSalesAlertsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreSalesAlertRoutingModule {}
