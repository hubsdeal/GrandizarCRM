import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreBusinessCustomerMapRoutingModule } from './storeBusinessCustomerMap-routing.module';
import { StoreBusinessCustomerMapsComponent } from './storeBusinessCustomerMaps.component';
import { CreateOrEditStoreBusinessCustomerMapModalComponent } from './create-or-edit-storeBusinessCustomerMap-modal.component';
import { ViewStoreBusinessCustomerMapModalComponent } from './view-storeBusinessCustomerMap-modal.component';
import { StoreBusinessCustomerMapStoreLookupTableModalComponent } from './storeBusinessCustomerMap-store-lookup-table-modal.component';
import { StoreBusinessCustomerMapBusinessLookupTableModalComponent } from './storeBusinessCustomerMap-business-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreBusinessCustomerMapsComponent,
        CreateOrEditStoreBusinessCustomerMapModalComponent,
        ViewStoreBusinessCustomerMapModalComponent,

        StoreBusinessCustomerMapStoreLookupTableModalComponent,
        StoreBusinessCustomerMapBusinessLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreBusinessCustomerMapRoutingModule, AdminSharedModule],
})
export class StoreBusinessCustomerMapModule {}
