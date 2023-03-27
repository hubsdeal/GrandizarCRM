import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreRelevantStoresComponent } from './storeRelevantStores.component';

const routes: Routes = [
    {
        path: '',
        component: StoreRelevantStoresComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreRelevantStoreRoutingModule {}
