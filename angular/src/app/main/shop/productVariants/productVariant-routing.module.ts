import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductVariantsComponent } from './productVariants.component';

const routes: Routes = [
    {
        path: '',
        component: ProductVariantsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductVariantRoutingModule {}
