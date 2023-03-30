import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductVariantRoutingModule } from './productVariant-routing.module';
import { ProductVariantsComponent } from './productVariants.component';
import { CreateOrEditProductVariantModalComponent } from './create-or-edit-productVariant-modal.component';
import { ViewProductVariantModalComponent } from './view-productVariant-modal.component';
import { ProductVariantProductVariantCategoryLookupTableModalComponent } from './productVariant-productVariantCategory-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductVariantsComponent,
        CreateOrEditProductVariantModalComponent,
        ViewProductVariantModalComponent,

        ProductVariantProductVariantCategoryLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductVariantRoutingModule, AdminSharedModule],
})
export class ProductVariantModule {}
