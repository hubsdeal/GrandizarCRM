import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderDeliveryInfosComponent } from './orderDeliveryInfos.component';

const routes: Routes = [
    {
        path: '',
        component: OrderDeliveryInfosComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class OrderDeliveryInfoRoutingModule {}
