import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderFulfillmentStatusesComponent } from './orderFulfillmentStatuses.component';

const routes: Routes = [
    {
        path: '',
        component: OrderFulfillmentStatusesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class OrderFulfillmentStatusRoutingModule {}
