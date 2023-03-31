import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductAndGiftCardMapsComponent } from './productAndGiftCardMaps.component';

const routes: Routes = [
    {
        path: '',
        component: ProductAndGiftCardMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductAndGiftCardMapRoutingModule {}
