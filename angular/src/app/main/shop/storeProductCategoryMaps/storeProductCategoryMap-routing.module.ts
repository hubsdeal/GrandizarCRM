import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreProductCategoryMapsComponent } from './storeProductCategoryMaps.component';

const routes: Routes = [
    {
        path: '',
        component: StoreProductCategoryMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreProductCategoryMapRoutingModule {}
