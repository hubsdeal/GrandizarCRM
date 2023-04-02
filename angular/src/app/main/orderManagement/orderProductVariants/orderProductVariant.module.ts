import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { OrderProductVariantRoutingModule } from './orderProductVariant-routing.module';
import { OrderProductVariantsComponent } from './orderProductVariants.component';
import { CreateOrEditOrderProductVariantModalComponent } from './create-or-edit-orderProductVariant-modal.component';
import { ViewOrderProductVariantModalComponent } from './view-orderProductVariant-modal.component';
import { OrderProductVariantProductVariantCategoryLookupTableModalComponent } from './orderProductVariant-productVariantCategory-lookup-table-modal.component';
import { OrderProductVariantProductVariantLookupTableModalComponent } from './orderProductVariant-productVariant-lookup-table-modal.component';

@NgModule({
    declarations: [
        OrderProductVariantsComponent,
        CreateOrEditOrderProductVariantModalComponent,
        ViewOrderProductVariantModalComponent,

        OrderProductVariantProductVariantCategoryLookupTableModalComponent,
        OrderProductVariantProductVariantLookupTableModalComponent,
    ],
    imports: [AppSharedModule, OrderProductVariantRoutingModule, AdminSharedModule],
})
export class OrderProductVariantModule {}
