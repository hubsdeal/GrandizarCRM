import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductCustomerQueriesComponent } from './productCustomerQueries.component';

const routes: Routes = [
    {
        path: '',
        component: ProductCustomerQueriesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductCustomerQueryRoutingModule {}
