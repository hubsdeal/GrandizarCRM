import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreZipCodeMapsComponent } from './storeZipCodeMaps.component';

const routes: Routes = [
    {
        path: '',
        component: StoreZipCodeMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreZipCodeMapRoutingModule {}
