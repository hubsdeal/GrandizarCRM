import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { OrderDeliveryInfoRoutingModule } from './orderDeliveryInfo-routing.module';
import { OrderDeliveryInfosComponent } from './orderDeliveryInfos.component';
import { CreateOrEditOrderDeliveryInfoModalComponent } from './create-or-edit-orderDeliveryInfo-modal.component';
import { ViewOrderDeliveryInfoModalComponent } from './view-orderDeliveryInfo-modal.component';
import { OrderDeliveryInfoEmployeeLookupTableModalComponent } from './orderDeliveryInfo-employee-lookup-table-modal.component';
import { OrderDeliveryInfoOrderLookupTableModalComponent } from './orderDeliveryInfo-order-lookup-table-modal.component';

@NgModule({
    declarations: [
        OrderDeliveryInfosComponent,
        CreateOrEditOrderDeliveryInfoModalComponent,
        ViewOrderDeliveryInfoModalComponent,

        OrderDeliveryInfoEmployeeLookupTableModalComponent,
        OrderDeliveryInfoOrderLookupTableModalComponent,
    ],
    imports: [AppSharedModule, OrderDeliveryInfoRoutingModule, AdminSharedModule],
})
export class OrderDeliveryInfoModule {}
