import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CustomerWalletTransactionRoutingModule} from './customerWalletTransaction-routing.module';
import {CustomerWalletTransactionsComponent} from './customerWalletTransactions.component';
import {CreateOrEditCustomerWalletTransactionModalComponent} from './create-or-edit-customerWalletTransaction-modal.component';
import {ViewCustomerWalletTransactionModalComponent} from './view-customerWalletTransaction-modal.component';
import {CustomerWalletTransactionOrderLookupTableModalComponent} from './customerWalletTransaction-order-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        CustomerWalletTransactionsComponent,
        CreateOrEditCustomerWalletTransactionModalComponent,
        ViewCustomerWalletTransactionModalComponent,
        
    					CustomerWalletTransactionOrderLookupTableModalComponent,
    ],
    imports: [AppSharedModule, CustomerWalletTransactionRoutingModule , AdminSharedModule ],
    
})
export class CustomerWalletTransactionModule {
}
