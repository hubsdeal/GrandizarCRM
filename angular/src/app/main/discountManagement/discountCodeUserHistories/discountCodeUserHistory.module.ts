import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DiscountCodeUserHistoryRoutingModule } from './discountCodeUserHistory-routing.module';
import { DiscountCodeUserHistoriesComponent } from './discountCodeUserHistories.component';
import { CreateOrEditDiscountCodeUserHistoryModalComponent } from './create-or-edit-discountCodeUserHistory-modal.component';
import { ViewDiscountCodeUserHistoryModalComponent } from './view-discountCodeUserHistory-modal.component';
import { DiscountCodeUserHistoryDiscountCodeGeneratorLookupTableModalComponent } from './discountCodeUserHistory-discountCodeGenerator-lookup-table-modal.component';
import { DiscountCodeUserHistoryOrderLookupTableModalComponent } from './discountCodeUserHistory-order-lookup-table-modal.component';
import { DiscountCodeUserHistoryContactLookupTableModalComponent } from './discountCodeUserHistory-contact-lookup-table-modal.component';

@NgModule({
    declarations: [
        DiscountCodeUserHistoriesComponent,
        CreateOrEditDiscountCodeUserHistoryModalComponent,
        ViewDiscountCodeUserHistoryModalComponent,

        DiscountCodeUserHistoryDiscountCodeGeneratorLookupTableModalComponent,
        DiscountCodeUserHistoryOrderLookupTableModalComponent,
        DiscountCodeUserHistoryContactLookupTableModalComponent,
    ],
    imports: [AppSharedModule, DiscountCodeUserHistoryRoutingModule, AdminSharedModule],
})
export class DiscountCodeUserHistoryModule {}
