import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductByVariantsComponent } from './productByVariants.component';

const routes: Routes = [
    {
        path: '',
        component: ProductByVariantsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductByVariantRoutingModule {}
