import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessStoreMapsComponent } from './businessStoreMaps.component';

const routes: Routes = [
    {
        path: '',
        component: BusinessStoreMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BusinessStoreMapRoutingModule {}
