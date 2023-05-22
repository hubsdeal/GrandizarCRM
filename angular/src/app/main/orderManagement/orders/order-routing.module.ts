import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrdersComponent } from './orders.component';
import { OrderDashboardComponent } from './order-dashboard/order-dashboard.component';

const routes: Routes = [
    {
        path: '',
        component: OrdersComponent,
        pathMatch: 'full',
    },
    {
        path: 'dashboard/:orderId',
        component: OrderDashboardComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class OrderRoutingModule {}
