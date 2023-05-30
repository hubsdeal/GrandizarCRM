import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreWidgetContentMapsComponent } from './storeWidgetContentMaps.component';

const routes: Routes = [
    {
        path: '',
        component: StoreWidgetContentMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreWidgetContentMapRoutingModule {}
