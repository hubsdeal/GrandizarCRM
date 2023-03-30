import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductWholeSaleQuantityTypesComponent } from './productWholeSaleQuantityTypes.component';

const routes: Routes = [
    {
        path: '',
        component: ProductWholeSaleQuantityTypesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductWholeSaleQuantityTypeRoutingModule {}
