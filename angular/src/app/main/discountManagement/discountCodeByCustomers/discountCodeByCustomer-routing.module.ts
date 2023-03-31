import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DiscountCodeByCustomersComponent } from './discountCodeByCustomers.component';

const routes: Routes = [
    {
        path: '',
        component: DiscountCodeByCustomersComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DiscountCodeByCustomerRoutingModule {}
