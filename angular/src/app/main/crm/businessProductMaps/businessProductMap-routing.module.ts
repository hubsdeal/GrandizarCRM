import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessProductMapsComponent } from './businessProductMaps.component';

const routes: Routes = [
    {
        path: '',
        component: BusinessProductMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BusinessProductMapRoutingModule {}
