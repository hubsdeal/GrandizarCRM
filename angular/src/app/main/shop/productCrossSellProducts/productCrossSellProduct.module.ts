import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductCrossSellProductRoutingModule } from './productCrossSellProduct-routing.module';
import { ProductCrossSellProductsComponent } from './productCrossSellProducts.component';
import { CreateOrEditProductCrossSellProductModalComponent } from './create-or-edit-productCrossSellProduct-modal.component';
import { ViewProductCrossSellProductModalComponent } from './view-productCrossSellProduct-modal.component';
import { ProductCrossSellProductProductLookupTableModalComponent } from './productCrossSellProduct-product-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductCrossSellProductsComponent,
        CreateOrEditProductCrossSellProductModalComponent,
        ViewProductCrossSellProductModalComponent,

        ProductCrossSellProductProductLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductCrossSellProductRoutingModule, AdminSharedModule],
})
export class ProductCrossSellProductModule {}
