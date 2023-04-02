import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderProductVariantsComponent } from './orderProductVariants.component';

const routes: Routes = [
    {
        path: '',
        component: OrderProductVariantsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class OrderProductVariantRoutingModule {}
