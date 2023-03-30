import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductMediasComponent } from './productMedias.component';

const routes: Routes = [
    {
        path: '',
        component: ProductMediasComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductMediaRoutingModule {}
