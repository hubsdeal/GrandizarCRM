import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductCategoryMapsComponent } from './productCategoryMaps.component';

const routes: Routes = [
    {
        path: '',
        component: ProductCategoryMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductCategoryMapRoutingModule {}
