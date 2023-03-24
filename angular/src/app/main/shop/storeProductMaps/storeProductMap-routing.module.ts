import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreProductMapsComponent } from './storeProductMaps.component';

const routes: Routes = [
    {
        path: '',
        component: StoreProductMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreProductMapRoutingModule {}
