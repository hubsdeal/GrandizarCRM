import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductCustomerStatsComponent } from './productCustomerStats.component';

const routes: Routes = [
    {
        path: '',
        component: ProductCustomerStatsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductCustomerStatRoutingModule {}
