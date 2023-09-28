import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {OrderTaskMapRoutingModule} from './orderTaskMap-routing.module';
import {OrderTaskMapsComponent} from './orderTaskMaps.component';
import {CreateOrEditOrderTaskMapModalComponent} from './create-or-edit-orderTaskMap-modal.component';
import {ViewOrderTaskMapModalComponent} from './view-orderTaskMap-modal.component';
import {OrderTaskMapOrderLookupTableModalComponent} from './orderTaskMap-order-lookup-table-modal.component';
    					import {OrderTaskMapTaskEventLookupTableModalComponent} from './orderTaskMap-taskEvent-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        OrderTaskMapsComponent,
        CreateOrEditOrderTaskMapModalComponent,
        ViewOrderTaskMapModalComponent,
        
    					OrderTaskMapOrderLookupTableModalComponent,
    					OrderTaskMapTaskEventLookupTableModalComponent,
    ],
    imports: [AppSharedModule, OrderTaskMapRoutingModule , AdminSharedModule ],
    
})
export class OrderTaskMapModule {
}
