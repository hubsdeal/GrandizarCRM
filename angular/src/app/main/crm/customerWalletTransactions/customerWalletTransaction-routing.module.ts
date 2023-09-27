import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CustomerWalletTransactionsComponent} from './customerWalletTransactions.component';



const routes: Routes = [
    {
        path: '',
        component: CustomerWalletTransactionsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CustomerWalletTransactionRoutingModule {
}
