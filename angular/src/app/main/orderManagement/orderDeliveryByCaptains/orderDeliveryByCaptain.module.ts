import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {OrderDeliveryByCaptainRoutingModule} from './orderDeliveryByCaptain-routing.module';
import {OrderDeliveryByCaptainsComponent} from './orderDeliveryByCaptains.component';
import {CreateOrEditOrderDeliveryByCaptainModalComponent} from './create-or-edit-orderDeliveryByCaptain-modal.component';
import {ViewOrderDeliveryByCaptainModalComponent} from './view-orderDeliveryByCaptain-modal.component';
import {OrderDeliveryByCaptainOrderLookupTableModalComponent} from './orderDeliveryByCaptain-order-lookup-table-modal.component';
    					import {OrderDeliveryByCaptainStoreLookupTableModalComponent} from './orderDeliveryByCaptain-store-lookup-table-modal.component';
    					import {OrderDeliveryByCaptainContactLookupTableModalComponent} from './orderDeliveryByCaptain-contact-lookup-table-modal.component';
    					import {OrderDeliveryByCaptainEmployeeLookupTableModalComponent} from './orderDeliveryByCaptain-employee-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        OrderDeliveryByCaptainsComponent,
        CreateOrEditOrderDeliveryByCaptainModalComponent,
        ViewOrderDeliveryByCaptainModalComponent,
        
    					OrderDeliveryByCaptainOrderLookupTableModalComponent,
    					OrderDeliveryByCaptainStoreLookupTableModalComponent,
    					OrderDeliveryByCaptainContactLookupTableModalComponent,
    					OrderDeliveryByCaptainEmployeeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, OrderDeliveryByCaptainRoutingModule , AdminSharedModule ],
    
})
export class OrderDeliveryByCaptainModule {
}
