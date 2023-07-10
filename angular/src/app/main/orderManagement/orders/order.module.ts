import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { OrderRoutingModule } from './order-routing.module';
import { OrdersComponent } from './orders.component';
import { CreateOrEditOrderModalComponent } from './create-or-edit-order-modal.component';
import { ViewOrderModalComponent } from './view-order-modal.component';
import { OrderStateLookupTableModalComponent } from './order-state-lookup-table-modal.component';
import { OrderCountryLookupTableModalComponent } from './order-country-lookup-table-modal.component';
import { OrderContactLookupTableModalComponent } from './order-contact-lookup-table-modal.component';
import { OrderOrderStatusLookupTableModalComponent } from './order-orderStatus-lookup-table-modal.component';
import { OrderCurrencyLookupTableModalComponent } from './order-currency-lookup-table-modal.component';
import { OrderStoreLookupTableModalComponent } from './order-store-lookup-table-modal.component';
import { OrderOrderSalesChannelLookupTableModalComponent } from './order-orderSalesChannel-lookup-table-modal.component';
import { OrderDashboardComponent } from './order-dashboard/order-dashboard.component';
import { MyOrdersComponent } from './my-orders/my-orders.component';
import { AbandonedCartComponent } from './abandoned-cart/abandoned-cart.component';

@NgModule({
    declarations: [
        OrdersComponent,
        CreateOrEditOrderModalComponent,
        ViewOrderModalComponent,

        OrderStateLookupTableModalComponent,
        OrderCountryLookupTableModalComponent,
        OrderContactLookupTableModalComponent,
        OrderOrderStatusLookupTableModalComponent,
        OrderCurrencyLookupTableModalComponent,
        OrderStoreLookupTableModalComponent,
        OrderOrderSalesChannelLookupTableModalComponent,
        OrderDashboardComponent,
        MyOrdersComponent,
        AbandonedCartComponent,
    ],
    imports: [AppSharedModule, OrderRoutingModule, AdminSharedModule],
    exports: [OrdersComponent,
        CreateOrEditOrderModalComponent,
        ViewOrderModalComponent,

        OrderStateLookupTableModalComponent,
        OrderCountryLookupTableModalComponent,
        OrderContactLookupTableModalComponent,
        OrderOrderStatusLookupTableModalComponent,
        OrderCurrencyLookupTableModalComponent,
        OrderStoreLookupTableModalComponent,
        OrderOrderSalesChannelLookupTableModalComponent,
        OrderDashboardComponent,
        MyOrdersComponent,
        AbandonedCartComponent],
})
export class OrderModule {}
