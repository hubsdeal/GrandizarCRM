import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HubWidgetStoreMapsComponent } from './hubWidgetStoreMaps.component';

const routes: Routes = [
    {
        path: '',
        component: HubWidgetStoreMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubWidgetStoreMapRoutingModule {}
