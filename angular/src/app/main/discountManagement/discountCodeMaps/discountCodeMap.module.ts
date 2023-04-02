import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DiscountCodeMapRoutingModule } from './discountCodeMap-routing.module';
import { DiscountCodeMapsComponent } from './discountCodeMaps.component';
import { CreateOrEditDiscountCodeMapModalComponent } from './create-or-edit-discountCodeMap-modal.component';
import { ViewDiscountCodeMapModalComponent } from './view-discountCodeMap-modal.component';
import { DiscountCodeMapDiscountCodeGeneratorLookupTableModalComponent } from './discountCodeMap-discountCodeGenerator-lookup-table-modal.component';
import { DiscountCodeMapStoreLookupTableModalComponent } from './discountCodeMap-store-lookup-table-modal.component';
import { DiscountCodeMapProductLookupTableModalComponent } from './discountCodeMap-product-lookup-table-modal.component';
import { DiscountCodeMapMembershipTypeLookupTableModalComponent } from './discountCodeMap-membershipType-lookup-table-modal.component';

@NgModule({
    declarations: [
        DiscountCodeMapsComponent,
        CreateOrEditDiscountCodeMapModalComponent,
        ViewDiscountCodeMapModalComponent,

        DiscountCodeMapDiscountCodeGeneratorLookupTableModalComponent,
        DiscountCodeMapStoreLookupTableModalComponent,
        DiscountCodeMapProductLookupTableModalComponent,
        DiscountCodeMapMembershipTypeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, DiscountCodeMapRoutingModule, AdminSharedModule],
})
export class DiscountCodeMapModule {}
