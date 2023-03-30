import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductCategoryMapRoutingModule } from './productCategoryMap-routing.module';
import { ProductCategoryMapsComponent } from './productCategoryMaps.component';
import { CreateOrEditProductCategoryMapModalComponent } from './create-or-edit-productCategoryMap-modal.component';
import { ViewProductCategoryMapModalComponent } from './view-productCategoryMap-modal.component';
import { ProductCategoryMapProductLookupTableModalComponent } from './productCategoryMap-product-lookup-table-modal.component';
import { ProductCategoryMapProductCategoryLookupTableModalComponent } from './productCategoryMap-productCategory-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductCategoryMapsComponent,
        CreateOrEditProductCategoryMapModalComponent,
        ViewProductCategoryMapModalComponent,

        ProductCategoryMapProductLookupTableModalComponent,
        ProductCategoryMapProductCategoryLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductCategoryMapRoutingModule, AdminSharedModule],
})
export class ProductCategoryMapModule {}
