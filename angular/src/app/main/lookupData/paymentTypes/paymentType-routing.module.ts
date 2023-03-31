import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PaymentTypesComponent } from './paymentTypes.component';

const routes: Routes = [
    {
        path: '',
        component: PaymentTypesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PaymentTypeRoutingModule {}
