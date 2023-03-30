import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductPackagesComponent } from './productPackages.component';

const routes: Routes = [
    {
        path: '',
        component: ProductPackagesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductPackageRoutingModule {}
