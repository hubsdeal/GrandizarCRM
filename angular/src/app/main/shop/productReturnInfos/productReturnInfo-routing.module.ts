import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductReturnInfosComponent } from './productReturnInfos.component';

const routes: Routes = [
    {
        path: '',
        component: ProductReturnInfosComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductReturnInfoRoutingModule {}
