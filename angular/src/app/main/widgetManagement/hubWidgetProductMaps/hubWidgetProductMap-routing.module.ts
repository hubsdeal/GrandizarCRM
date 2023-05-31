import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HubWidgetProductMapsComponent } from './hubWidgetProductMaps.component';

const routes: Routes = [
    {
        path: '',
        component: HubWidgetProductMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubWidgetProductMapRoutingModule {}
