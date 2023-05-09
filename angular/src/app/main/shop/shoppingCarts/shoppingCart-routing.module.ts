import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShoppingCartsComponent } from './shoppingCarts.component';

const routes: Routes = [
    {
        path: '',
        component: ShoppingCartsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ShoppingCartRoutingModule {}
