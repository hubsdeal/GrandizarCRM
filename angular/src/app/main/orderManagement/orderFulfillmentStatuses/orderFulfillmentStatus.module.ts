import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { OrderFulfillmentStatusRoutingModule } from './orderFulfillmentStatus-routing.module';
import { OrderFulfillmentStatusesComponent } from './orderFulfillmentStatuses.component';
import { CreateOrEditOrderFulfillmentStatusModalComponent } from './create-or-edit-orderFulfillmentStatus-modal.component';
import { ViewOrderFulfillmentStatusModalComponent } from './view-orderFulfillmentStatus-modal.component';
import { OrderFulfillmentStatusOrderStatusLookupTableModalComponent } from './orderFulfillmentStatus-orderStatus-lookup-table-modal.component';
import { OrderFulfillmentStatusOrderLookupTableModalComponent } from './orderFulfillmentStatus-order-lookup-table-modal.component';
import { OrderFulfillmentStatusEmployeeLookupTableModalComponent } from './orderFulfillmentStatus-employee-lookup-table-modal.component';

@NgModule({
    declarations: [
        OrderFulfillmentStatusesComponent,
        CreateOrEditOrderFulfillmentStatusModalComponent,
        ViewOrderFulfillmentStatusModalComponent,

        OrderFulfillmentStatusOrderStatusLookupTableModalComponent,
        OrderFulfillmentStatusOrderLookupTableModalComponent,
        OrderFulfillmentStatusEmployeeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, OrderFulfillmentStatusRoutingModule, AdminSharedModule],
})
export class OrderFulfillmentStatusModule {}
