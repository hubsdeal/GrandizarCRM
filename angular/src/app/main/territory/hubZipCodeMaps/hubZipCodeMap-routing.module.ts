import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HubZipCodeMapsComponent } from './hubZipCodeMaps.component';

const routes: Routes = [
    {
        path: '',
        component: HubZipCodeMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubZipCodeMapRoutingModule {}
