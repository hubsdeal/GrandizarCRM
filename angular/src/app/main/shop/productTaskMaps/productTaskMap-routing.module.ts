import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductTaskMapsComponent } from './productTaskMaps.component';

const routes: Routes = [
    {
        path: '',
        component: ProductTaskMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductTaskMapRoutingModule {}
