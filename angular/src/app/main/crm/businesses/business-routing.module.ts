import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessDashboardComponent } from './business-dashboard/business-dashboard.component';
import { BusinessesComponent } from './businesses.component';

const routes: Routes = [
    {
        path: '',
        component: BusinessesComponent,
        pathMatch: 'full',
    },
    {
        path: 'dashboard/:businessId',
        component: BusinessDashboardComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BusinessRoutingModule {}
