import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductCategoriesComponent } from './productCategories.component';

const routes: Routes = [
    {
        path: '',
        component: ProductCategoriesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductCategoryRoutingModule {}
