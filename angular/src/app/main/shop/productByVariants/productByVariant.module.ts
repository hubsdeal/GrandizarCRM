import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductByVariantRoutingModule } from './productByVariant-routing.module';
import { ProductByVariantsComponent } from './productByVariants.component';
import { CreateOrEditProductByVariantModalComponent } from './create-or-edit-productByVariant-modal.component';
import { ViewProductByVariantModalComponent } from './view-productByVariant-modal.component';
import { ProductByVariantProductLookupTableModalComponent } from './productByVariant-product-lookup-table-modal.component';
import { ProductByVariantProductVariantLookupTableModalComponent } from './productByVariant-productVariant-lookup-table-modal.component';
import { ProductByVariantProductVariantCategoryLookupTableModalComponent } from './productByVariant-productVariantCategory-lookup-table-modal.component';
import { ProductByVariantMediaLibraryLookupTableModalComponent } from './productByVariant-mediaLibrary-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductByVariantsComponent,
        CreateOrEditProductByVariantModalComponent,
        ViewProductByVariantModalComponent,

        ProductByVariantProductLookupTableModalComponent,
        ProductByVariantProductVariantLookupTableModalComponent,
        ProductByVariantProductVariantCategoryLookupTableModalComponent,
        ProductByVariantMediaLibraryLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductByVariantRoutingModule, AdminSharedModule],
})
export class ProductByVariantModule {}
