import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreLocationsComponent } from './storeLocations.component';

const routes: Routes = [
    {
        path: '',
        component: StoreLocationsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreLocationRoutingModule {}
