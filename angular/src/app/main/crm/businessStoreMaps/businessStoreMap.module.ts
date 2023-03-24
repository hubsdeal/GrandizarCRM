import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BusinessStoreMapRoutingModule } from './businessStoreMap-routing.module';
import { BusinessStoreMapsComponent } from './businessStoreMaps.component';
import { CreateOrEditBusinessStoreMapModalComponent } from './create-or-edit-businessStoreMap-modal.component';
import { ViewBusinessStoreMapModalComponent } from './view-businessStoreMap-modal.component';
import { BusinessStoreMapBusinessLookupTableModalComponent } from './businessStoreMap-business-lookup-table-modal.component';
import { BusinessStoreMapStoreLookupTableModalComponent } from './businessStoreMap-store-lookup-table-modal.component';

@NgModule({
    declarations: [
        BusinessStoreMapsComponent,
        CreateOrEditBusinessStoreMapModalComponent,
        ViewBusinessStoreMapModalComponent,

        BusinessStoreMapBusinessLookupTableModalComponent,
        BusinessStoreMapStoreLookupTableModalComponent,
    ],
    imports: [AppSharedModule, BusinessStoreMapRoutingModule, AdminSharedModule],
})
export class BusinessStoreMapModule {}
