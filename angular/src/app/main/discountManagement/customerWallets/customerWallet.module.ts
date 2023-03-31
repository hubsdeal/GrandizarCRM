import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CustomerWalletRoutingModule } from './customerWallet-routing.module';
import { CustomerWalletsComponent } from './customerWallets.component';
import { CreateOrEditCustomerWalletModalComponent } from './create-or-edit-customerWallet-modal.component';
import { ViewCustomerWalletModalComponent } from './view-customerWallet-modal.component';
import { CustomerWalletContactLookupTableModalComponent } from './customerWallet-contact-lookup-table-modal.component';
import { CustomerWalletUserLookupTableModalComponent } from './customerWallet-user-lookup-table-modal.component';
import { CustomerWalletCurrencyLookupTableModalComponent } from './customerWallet-currency-lookup-table-modal.component';

@NgModule({
    declarations: [
        CustomerWalletsComponent,
        CreateOrEditCustomerWalletModalComponent,
        ViewCustomerWalletModalComponent,

        CustomerWalletContactLookupTableModalComponent,
        CustomerWalletUserLookupTableModalComponent,
        CustomerWalletCurrencyLookupTableModalComponent,
    ],
    imports: [AppSharedModule, CustomerWalletRoutingModule, AdminSharedModule],
})
export class CustomerWalletModule {}
