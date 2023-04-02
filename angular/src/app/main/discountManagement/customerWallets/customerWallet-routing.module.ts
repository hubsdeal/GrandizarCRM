import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerWalletsComponent } from './customerWallets.component';

const routes: Routes = [
    {
        path: '',
        component: CustomerWalletsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CustomerWalletRoutingModule {}
