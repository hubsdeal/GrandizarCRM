import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { OrderSalesChannelRoutingModule } from './orderSalesChannel-routing.module';
import { OrderSalesChannelsComponent } from './orderSalesChannels.component';
import { CreateOrEditOrderSalesChannelModalComponent } from './create-or-edit-orderSalesChannel-modal.component';
import { ViewOrderSalesChannelModalComponent } from './view-orderSalesChannel-modal.component';

@NgModule({
    declarations: [
        OrderSalesChannelsComponent,
        CreateOrEditOrderSalesChannelModalComponent,
        ViewOrderSalesChannelModalComponent,
    ],
    imports: [AppSharedModule, OrderSalesChannelRoutingModule, AdminSharedModule],
})
export class OrderSalesChannelModule {}
