import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { OrderProductInfoRoutingModule } from './orderProductInfo-routing.module';
import { OrderProductInfosComponent } from './orderProductInfos.component';
import { CreateOrEditOrderProductInfoModalComponent } from './create-or-edit-orderProductInfo-modal.component';
import { ViewOrderProductInfoModalComponent } from './view-orderProductInfo-modal.component';
import { OrderProductInfoOrderLookupTableModalComponent } from './orderProductInfo-order-lookup-table-modal.component';
import { OrderProductInfoStoreLookupTableModalComponent } from './orderProductInfo-store-lookup-table-modal.component';
import { OrderProductInfoProductLookupTableModalComponent } from './orderProductInfo-product-lookup-table-modal.component';
import { OrderProductInfoMeasurementUnitLookupTableModalComponent } from './orderProductInfo-measurementUnit-lookup-table-modal.component';

@NgModule({
    declarations: [
        OrderProductInfosComponent,
        CreateOrEditOrderProductInfoModalComponent,
        ViewOrderProductInfoModalComponent,

        OrderProductInfoOrderLookupTableModalComponent,
        OrderProductInfoStoreLookupTableModalComponent,
        OrderProductInfoProductLookupTableModalComponent,
        OrderProductInfoMeasurementUnitLookupTableModalComponent,
    ],
    imports: [AppSharedModule, OrderProductInfoRoutingModule, AdminSharedModule],
})
export class OrderProductInfoModule {}
