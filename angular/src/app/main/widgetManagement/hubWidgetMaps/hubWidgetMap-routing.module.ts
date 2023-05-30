import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HubWidgetMapsComponent } from './hubWidgetMaps.component';

const routes: Routes = [
    {
        path: '',
        component: HubWidgetMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubWidgetMapRoutingModule {}
