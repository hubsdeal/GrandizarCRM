import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HubSalesProjectionsComponent } from './hubSalesProjections.component';

const routes: Routes = [
    {
        path: '',
        component: HubSalesProjectionsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubSalesProjectionRoutingModule {}
