import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreDashboardComponent } from './store-dashboard/store-dashboard.component';
import { StoresComponent } from './stores.component';

const routes: Routes = [
    {
        path: '',
        component: StoresComponent,
        pathMatch: 'full',
    },
    {
        path: 'dashboard/:storeId',
        component: StoreDashboardComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreRoutingModule {}
