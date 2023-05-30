import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HubWidgetContentMapsComponent } from './hubWidgetContentMaps.component';

const routes: Routes = [
    {
        path: '',
        component: HubWidgetContentMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubWidgetContentMapRoutingModule {}
