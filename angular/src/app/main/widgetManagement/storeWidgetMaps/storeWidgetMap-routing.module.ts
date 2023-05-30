import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreWidgetMapsComponent } from './storeWidgetMaps.component';

const routes: Routes = [
    {
        path: '',
        component: StoreWidgetMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreWidgetMapRoutingModule {}
