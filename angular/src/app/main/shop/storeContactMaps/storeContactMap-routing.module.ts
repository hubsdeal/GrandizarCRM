import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreContactMapsComponent } from './storeContactMaps.component';

const routes: Routes = [
    {
        path: '',
        component: StoreContactMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreContactMapRoutingModule {}
