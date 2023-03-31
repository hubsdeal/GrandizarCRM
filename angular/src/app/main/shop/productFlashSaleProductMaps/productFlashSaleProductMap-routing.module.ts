import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductFlashSaleProductMapsComponent } from './productFlashSaleProductMaps.component';

const routes: Routes = [
    {
        path: '',
        component: ProductFlashSaleProductMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductFlashSaleProductMapRoutingModule {}
