import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessJobMapsComponent } from './businessJobMaps.component';

const routes: Routes = [
    {
        path: '',
        component: BusinessJobMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BusinessJobMapRoutingModule {}
