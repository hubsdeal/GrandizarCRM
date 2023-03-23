import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductDashboardComponent } from './product-dashboard/product-dashboard.component';
import { ProductsComponent } from './products.component';

const routes: Routes = [
    {
        path: '',
        component: ProductsComponent,
        pathMatch: 'full',
    },
    {
        path: 'dashboard/:productId',
        component: ProductDashboardComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductRoutingModule {}
