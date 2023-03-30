import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductFlashSaleProductMapRoutingModule } from './productFlashSaleProductMap-routing.module';
import { ProductFlashSaleProductMapsComponent } from './productFlashSaleProductMaps.component';
import { CreateOrEditProductFlashSaleProductMapModalComponent } from './create-or-edit-productFlashSaleProductMap-modal.component';
import { ViewProductFlashSaleProductMapModalComponent } from './view-productFlashSaleProductMap-modal.component';
import { ProductFlashSaleProductMapProductLookupTableModalComponent } from './productFlashSaleProductMap-product-lookup-table-modal.component';
import { ProductFlashSaleProductMapStoreLookupTableModalComponent } from './productFlashSaleProductMap-store-lookup-table-modal.component';
import { ProductFlashSaleProductMapMembershipTypeLookupTableModalComponent } from './productFlashSaleProductMap-membershipType-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductFlashSaleProductMapsComponent,
        CreateOrEditProductFlashSaleProductMapModalComponent,
        ViewProductFlashSaleProductMapModalComponent,

        ProductFlashSaleProductMapProductLookupTableModalComponent,
        ProductFlashSaleProductMapStoreLookupTableModalComponent,
        ProductFlashSaleProductMapMembershipTypeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductFlashSaleProductMapRoutingModule, AdminSharedModule],
})
export class ProductFlashSaleProductMapModule {}
