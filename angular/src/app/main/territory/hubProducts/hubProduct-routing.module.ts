import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HubProductsComponent } from './hubProducts.component';

const routes: Routes = [
    {
        path: '',
        component: HubProductsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubProductRoutingModule {}
