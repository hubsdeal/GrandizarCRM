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
    ],
    imports: [AppSharedModule, ProductRoutingModule, AdminSharedModule],
})
export class ProductModule {}
