import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductWholeSalePriceRoutingModule } from './productWholeSalePrice-routing.module';
import { ProductWholeSalePricesComponent } from './productWholeSalePrices.component';
import { CreateOrEditProductWholeSalePriceModalComponent } from './create-or-edit-productWholeSalePrice-modal.component';
import { ViewProductWholeSalePriceModalComponent } from './view-productWholeSalePrice-modal.component';
import { ProductWholeSalePriceProductLookupTableModalComponent } from './productWholeSalePrice-product-lookup-table-modal.component';
import { ProductWholeSalePriceProductWholeSaleQuantityTypeLookupTableModalComponent } from './productWholeSalePrice-productWholeSaleQuantityType-lookup-table-modal.component';
import { ProductWholeSalePriceMeasurementUnitLookupTableModalComponent } from './productWholeSalePrice-measurementUnit-lookup-table-modal.component';
import { ProductWholeSalePriceCurrencyLookupTableModalComponent } from './productWholeSalePrice-currency-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductWholeSalePricesComponent,
        CreateOrEditProductWholeSalePriceModalComponent,
        ViewProductWholeSalePriceModalComponent,

        ProductWholeSalePriceProductLookupTableModalComponent,
        ProductWholeSalePriceProductWholeSaleQuantityTypeLookupTableModalComponent,
        ProductWholeSalePriceMeasurementUnitLookupTableModalComponent,
        ProductWholeSalePriceCurrencyLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductWholeSalePriceRoutingModule, AdminSharedModule],
})
export class ProductWholeSalePriceModule {}
