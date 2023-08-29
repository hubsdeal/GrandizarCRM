import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductCategoriesComponent } from './productCategories.component';
import { ProductCategoryDashboardComponent } from './productCategoriesDashboard/product-category-dashboard/product-category-dashboard.component';

const routes: Routes = [
    {
        path: '',
        component: ProductCategoriesComponent,
        pathMatch: 'full',
    },
    {
        path: 'dashboard/:productCategoryId',
        component: ProductCategoryDashboardComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductCategoryRoutingModule {}
