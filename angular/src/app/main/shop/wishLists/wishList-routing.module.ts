import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WishListsComponent } from './wishLists.component';

const routes: Routes = [
    {
        path: '',
        component: WishListsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class WishListRoutingModule {}
