import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderPaymentInfosComponent } from './orderPaymentInfos.component';

const routes: Routes = [
    {
        path: '',
        component: OrderPaymentInfosComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class OrderPaymentInfoRoutingModule {}
