import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderProductInfosComponent } from './orderProductInfos.component';

const routes: Routes = [
    {
        path: '',
        component: OrderProductInfosComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class OrderProductInfoRoutingModule {}
