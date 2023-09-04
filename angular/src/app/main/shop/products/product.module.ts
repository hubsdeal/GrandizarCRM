import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductRoutingModule } from './product-routing.module';
import { ProductsComponent } from './products.component';
import { CreateOrEditProductModalComponent } from './create-or-edit-product-modal.component';
import { ViewProductModalComponent } from './view-product-modal.component';
import { ProductProductCategoryLookupTableModalComponent } from './product-productCategory-lookup-table-modal.component';
import { ProductMediaLibraryLookupTableModalComponent } from './product-mediaLibrary-lookup-table-modal.component';
import { ProductMeasurementUnitLookupTableModalComponent } from './product-measurementUnit-lookup-table-modal.component';
import { ProductCurrencyLookupTableModalComponent } from './product-currency-lookup-table-modal.component';
import { ProductRatingLikeLookupTableModalComponent } from './product-ratingLike-lookup-table-modal.component';
import { ProductContactLookupTableModalComponent } from './product-contact-lookup-table-modal.component';
import { ProductStoreLookupTableModalComponent } from './product-store-lookup-table-modal.component';
import { ProductDashboardComponent } from './product-dashboard/product-dashboard.component';
import { ProductLibrariesComponent } from './product-libraries/product-libraries.component';
import { ProductTagModule } from '../productTags/productTag.module';
import { MyProductsComponent } from './my-products/my-products.component';
import { ProductMediaModule } from '../productMedias/productMedia.module';
import { OrderModule } from '@app/main/orderManagement/orders/order.module';
import { ContactModule } from '@app/main/crm/contacts/contact.module';
import { ProductTaskMapModule } from '../productTaskMaps/productTaskMap.module';
import { ProductNoteModule } from '../productNotes/productNote.module';
import { ProductAccountTeamModule } from '../productAccountTeams/productAccountTeam.module';
import { MediaLibraryModule } from '@app/main/lookupData/mediaLibraries/mediaLibrary.module';
import { CreateOrEditBulkProductMediaLibraryModalComponent } from './product-dashboard/create-or-edit-bulk-product-media-library-modal/create-or-edit-bulk-product-media-library-modal.component';
import { ProductCrossSellProductModule } from '../productCrossSellProducts/productCrossSellProduct.module';
import { ProductUpsellRelatedProductModule } from '../productUpsellRelatedProducts/productUpsellRelatedProduct.module';
import { LeadModule } from '@app/main/salesLeadManagement/leads/lead.module';
import { CategoryWiseProductTemplateComponent } from './product-libraries/category-wise-product-template/category-wise-product-template.component';
import { StoreProductMapModule } from '../storeProductMaps/storeProductMap.module';
import { ProductCustomerStatModule } from '../productCustomerStats/productCustomerStat.module';

@NgModule({
    declarations: [
        ProductsComponent,
        CreateOrEditProductModalComponent,
        ViewProductModalComponent,

        ProductProductCategoryLookupTableModalComponent,
        ProductMediaLibraryLookupTableModalComponent,
        ProductMeasurementUnitLookupTableModalComponent,
        ProductCurrencyLookupTableModalComponent,
        ProductRatingLikeLookupTableModalComponent,
        ProductContactLookupTableModalComponent,
        ProductStoreLookupTableModalComponent,
        ProductDashboardComponent,
        ProductLibrariesComponent,
        MyProductsComponent,
        CreateOrEditBulkProductMediaLibraryModalComponent,
        CategoryWiseProductTemplateComponent
    ],
    imports: [AppSharedModule, ProductRoutingModule, AdminSharedModule, ProductTagModule, ProductMediaModule,
        OrderModule, ContactModule, ProductTaskMapModule, ProductNoteModule,
        ProductAccountTeamModule,
        MediaLibraryModule,
        ProductCrossSellProductModule,
        ProductUpsellRelatedProductModule,
        StoreProductMapModule,
        ProductCustomerStatModule,
        LeadModule],
    exports: [ProductsComponent,
        CreateOrEditProductModalComponent,
        ViewProductModalComponent,

        ProductProductCategoryLookupTableModalComponent,
        ProductMediaLibraryLookupTableModalComponent,
        ProductMeasurementUnitLookupTableModalComponent,
        ProductCurrencyLookupTableModalComponent,
        ProductRatingLikeLookupTableModalComponent,
        ProductContactLookupTableModalComponent,
        ProductStoreLookupTableModalComponent,
        ProductDashboardComponent,
        ProductLibrariesComponent,
        MyProductsComponent,
        CreateOrEditBulkProductMediaLibraryModalComponent]
})
export class ProductModule { }
