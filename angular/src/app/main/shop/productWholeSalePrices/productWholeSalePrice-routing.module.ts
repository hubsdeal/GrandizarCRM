import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductWholeSalePricesComponent } from './productWholeSalePrices.component';

const routes: Routes = [
    {
        path: '',
        component: ProductWholeSalePricesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductWholeSalePriceRoutingModule {}
