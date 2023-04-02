import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DiscountCodeByCustomerRoutingModule } from './discountCodeByCustomer-routing.module';
import { DiscountCodeByCustomersComponent } from './discountCodeByCustomers.component';
import { CreateOrEditDiscountCodeByCustomerModalComponent } from './create-or-edit-discountCodeByCustomer-modal.component';
import { ViewDiscountCodeByCustomerModalComponent } from './view-discountCodeByCustomer-modal.component';
import { DiscountCodeByCustomerDiscountCodeGeneratorLookupTableModalComponent } from './discountCodeByCustomer-discountCodeGenerator-lookup-table-modal.component';
import { DiscountCodeByCustomerContactLookupTableModalComponent } from './discountCodeByCustomer-contact-lookup-table-modal.component';

@NgModule({
    declarations: [
        DiscountCodeByCustomersComponent,
        CreateOrEditDiscountCodeByCustomerModalComponent,
        ViewDiscountCodeByCustomerModalComponent,

        DiscountCodeByCustomerDiscountCodeGeneratorLookupTableModalComponent,
        DiscountCodeByCustomerContactLookupTableModalComponent,
    ],
    imports: [AppSharedModule, DiscountCodeByCustomerRoutingModule, AdminSharedModule],
})
export class DiscountCodeByCustomerModule {}
