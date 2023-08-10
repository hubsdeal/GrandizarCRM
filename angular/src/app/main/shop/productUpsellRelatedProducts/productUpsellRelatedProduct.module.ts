import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductUpsellRelatedProductRoutingModule } from './productUpsellRelatedProduct-routing.module';
import { ProductUpsellRelatedProductsComponent } from './productUpsellRelatedProducts.component';
import { CreateOrEditProductUpsellRelatedProductModalComponent } from './create-or-edit-productUpsellRelatedProduct-modal.component';
import { ViewProductUpsellRelatedProductModalComponent } from './view-productUpsellRelatedProduct-modal.component';
import { ProductUpsellRelatedProductProductLookupTableModalComponent } from './productUpsellRelatedProduct-product-lookup-table-modal.component';
import { SwiperModule } from 'swiper/angular';

@NgModule({
    declarations: [
        ProductUpsellRelatedProductsComponent,
        CreateOrEditProductUpsellRelatedProductModalComponent,
        ViewProductUpsellRelatedProductModalComponent,

        ProductUpsellRelatedProductProductLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductUpsellRelatedProductRoutingModule, AdminSharedModule, SwiperModule],
    exports: [
        ProductUpsellRelatedProductsComponent,
        CreateOrEditProductUpsellRelatedProductModalComponent,
        ViewProductUpsellRelatedProductModalComponent,

        ProductUpsellRelatedProductProductLookupTableModalComponent,
    ]
})
export class ProductUpsellRelatedProductModule {}
