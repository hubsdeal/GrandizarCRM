import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderSalesChannelsComponent } from './orderSalesChannels.component';

const routes: Routes = [
    {
        path: '',
        component: OrderSalesChannelsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class OrderSalesChannelRoutingModule {}
