import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { OrderPaymentInfoRoutingModule } from './orderPaymentInfo-routing.module';
import { OrderPaymentInfosComponent } from './orderPaymentInfos.component';
import { CreateOrEditOrderPaymentInfoModalComponent } from './create-or-edit-orderPaymentInfo-modal.component';
import { ViewOrderPaymentInfoModalComponent } from './view-orderPaymentInfo-modal.component';
import { OrderPaymentInfoOrderLookupTableModalComponent } from './orderPaymentInfo-order-lookup-table-modal.component';
import { OrderPaymentInfoCurrencyLookupTableModalComponent } from './orderPaymentInfo-currency-lookup-table-modal.component';
import { OrderPaymentInfoPaymentTypeLookupTableModalComponent } from './orderPaymentInfo-paymentType-lookup-table-modal.component';

@NgModule({
    declarations: [
        OrderPaymentInfosComponent,
        CreateOrEditOrderPaymentInfoModalComponent,
        ViewOrderPaymentInfoModalComponent,

        OrderPaymentInfoOrderLookupTableModalComponent,
        OrderPaymentInfoCurrencyLookupTableModalComponent,
        OrderPaymentInfoPaymentTypeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, OrderPaymentInfoRoutingModule, AdminSharedModule],
})
export class OrderPaymentInfoModule {}
