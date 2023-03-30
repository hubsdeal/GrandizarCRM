import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductCrossSellProductsComponent } from './productCrossSellProducts.component';

const routes: Routes = [
    {
        path: '',
        component: ProductCrossSellProductsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductCrossSellProductRoutingModule {}
