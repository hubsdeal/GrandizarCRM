import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DiscountCodeMapsComponent } from './discountCodeMaps.component';

const routes: Routes = [
    {
        path: '',
        component: DiscountCodeMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DiscountCodeMapRoutingModule {}
