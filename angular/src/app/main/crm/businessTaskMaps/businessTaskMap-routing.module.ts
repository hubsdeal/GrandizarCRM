import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessTaskMapsComponent } from './businessTaskMaps.component';

const routes: Routes = [
    {
        path: '',
        component: BusinessTaskMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BusinessTaskMapRoutingModule {}
