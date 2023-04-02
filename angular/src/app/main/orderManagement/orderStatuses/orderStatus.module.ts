import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { OrderStatusRoutingModule } from './orderStatus-routing.module';
import { OrderStatusesComponent } from './orderStatuses.component';
import { CreateOrEditOrderStatusModalComponent } from './create-or-edit-orderStatus-modal.component';
import { ViewOrderStatusModalComponent } from './view-orderStatus-modal.component';
import { OrderStatusRoleLookupTableModalComponent } from './orderStatus-role-lookup-table-modal.component';

@NgModule({
    declarations: [
        OrderStatusesComponent,
        CreateOrEditOrderStatusModalComponent,
        ViewOrderStatusModalComponent,

        OrderStatusRoleLookupTableModalComponent,
    ],
    imports: [AppSharedModule, OrderStatusRoutingModule, AdminSharedModule],
})
export class OrderStatusModule {}
