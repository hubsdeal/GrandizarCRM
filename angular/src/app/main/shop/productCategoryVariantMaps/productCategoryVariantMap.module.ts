import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductCategoryVariantMapRoutingModule } from './productCategoryVariantMap-routing.module';
import { ProductCategoryVariantMapsComponent } from './productCategoryVariantMaps.component';
import { CreateOrEditProductCategoryVariantMapModalComponent } from './create-or-edit-productCategoryVariantMap-modal.component';
import { ViewProductCategoryVariantMapModalComponent } from './view-productCategoryVariantMap-modal.component';
import { ProductCategoryVariantMapProductCategoryLookupTableModalComponent } from './productCategoryVariantMap-productCategory-lookup-table-modal.component';
import { ProductCategoryVariantMapProductVariantCategoryLookupTableModalComponent } from './productCategoryVariantMap-productVariantCategory-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductCategoryVariantMapsComponent,
        CreateOrEditProductCategoryVariantMapModalComponent,
        ViewProductCategoryVariantMapModalComponent,

        ProductCategoryVariantMapProductCategoryLookupTableModalComponent,
        ProductCategoryVariantMapProductVariantCategoryLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductCategoryVariantMapRoutingModule, AdminSharedModule],
})
export class ProductCategoryVariantMapModule {}
