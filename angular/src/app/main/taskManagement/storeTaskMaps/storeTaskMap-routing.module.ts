import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreTaskMapsComponent } from './storeTaskMaps.component';

const routes: Routes = [
    {
        path: '',
        component: StoreTaskMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreTaskMapRoutingModule {}
