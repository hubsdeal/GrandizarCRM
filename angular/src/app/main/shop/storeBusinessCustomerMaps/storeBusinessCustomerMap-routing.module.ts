import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreBusinessCustomerMapsComponent } from './storeBusinessCustomerMaps.component';

const routes: Routes = [
    {
        path: '',
        component: StoreBusinessCustomerMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreBusinessCustomerMapRoutingModule {}
