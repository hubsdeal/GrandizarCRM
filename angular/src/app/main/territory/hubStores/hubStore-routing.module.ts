import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HubStoresComponent } from './hubStores.component';

const routes: Routes = [
    {
        path: '',
        component: HubStoresComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubStoreRoutingModule {}
