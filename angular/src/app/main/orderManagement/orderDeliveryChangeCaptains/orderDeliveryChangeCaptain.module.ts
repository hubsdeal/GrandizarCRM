import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { OrderDeliveryChangeCaptainRoutingModule } from './orderDeliveryChangeCaptain-routing.module';
import { OrderDeliveryChangeCaptainsComponent } from './orderDeliveryChangeCaptains.component';
import { CreateOrEditOrderDeliveryChangeCaptainModalComponent } from './create-or-edit-orderDeliveryChangeCaptain-modal.component';
import { ViewOrderDeliveryChangeCaptainModalComponent } from './view-orderDeliveryChangeCaptain-modal.component';
import { OrderDeliveryChangeCaptainOrderDeliveryByCaptainLookupTableModalComponent } from './orderDeliveryChangeCaptain-orderDeliveryByCaptain-lookup-table-modal.component';
import { OrderDeliveryChangeCaptainEmployeeLookupTableModalComponent } from './orderDeliveryChangeCaptain-employee-lookup-table-modal.component';
//import { OrderDeliveryChangeCaptainEmployeeLookupTableModalComponent } from './orderDeliveryChangeCaptain-employee-lookup-table-modal.component';



@NgModule({
    declarations: [
        OrderDeliveryChangeCaptainsComponent,
        CreateOrEditOrderDeliveryChangeCaptainModalComponent,
        ViewOrderDeliveryChangeCaptainModalComponent,

        OrderDeliveryChangeCaptainOrderDeliveryByCaptainLookupTableModalComponent,
        OrderDeliveryChangeCaptainEmployeeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, OrderDeliveryChangeCaptainRoutingModule, AdminSharedModule],

})
export class OrderDeliveryChangeCaptainModule {
}
