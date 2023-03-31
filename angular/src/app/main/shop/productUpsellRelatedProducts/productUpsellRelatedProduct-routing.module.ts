import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductUpsellRelatedProductsComponent } from './productUpsellRelatedProducts.component';

const routes: Routes = [
    {
        path: '',
        component: ProductUpsellRelatedProductsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductUpsellRelatedProductRoutingModule {}
