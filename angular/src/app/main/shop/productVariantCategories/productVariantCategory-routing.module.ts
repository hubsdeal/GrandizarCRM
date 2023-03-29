import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductVariantCategoriesComponent } from './productVariantCategories.component';

const routes: Routes = [
    {
        path: '',
        component: ProductVariantCategoriesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductVariantCategoryRoutingModule {}
