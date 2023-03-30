import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductSubscriptionMapsComponent } from './productSubscriptionMaps.component';

const routes: Routes = [
    {
        path: '',
        component: ProductSubscriptionMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductSubscriptionMapRoutingModule {}
