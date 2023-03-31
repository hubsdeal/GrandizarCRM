import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductCategoryVariantMapsComponent } from './productCategoryVariantMaps.component';

const routes: Routes = [
    {
        path: '',
        component: ProductCategoryVariantMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductCategoryVariantMapRoutingModule {}
