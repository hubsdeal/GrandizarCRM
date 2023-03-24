import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreBankAccountRoutingModule } from './storeBankAccount-routing.module';
import { StoreBankAccountsComponent } from './storeBankAccounts.component';
import { CreateOrEditStoreBankAccountModalComponent } from './create-or-edit-storeBankAccount-modal.component';
import { ViewStoreBankAccountModalComponent } from './view-storeBankAccount-modal.component';
import { StoreBankAccountStoreLookupTableModalComponent } from './storeBankAccount-store-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreBankAccountsComponent,
        CreateOrEditStoreBankAccountModalComponent,
        ViewStoreBankAccountModalComponent,

        StoreBankAccountStoreLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreBankAccountRoutingModule, AdminSharedModule],
})
export class StoreBankAccountModule {}
