import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreMediasComponent } from './storeMedias.component';

const routes: Routes = [
    {
        path: '',
        component: StoreMediasComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreMediaRoutingModule {}
