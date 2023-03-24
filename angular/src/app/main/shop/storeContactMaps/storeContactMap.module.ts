import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreContactMapRoutingModule } from './storeContactMap-routing.module';
import { StoreContactMapsComponent } from './storeContactMaps.component';
import { CreateOrEditStoreContactMapModalComponent } from './create-or-edit-storeContactMap-modal.component';
import { ViewStoreContactMapModalComponent } from './view-storeContactMap-modal.component';
import { StoreContactMapStoreLookupTableModalComponent } from './storeContactMap-store-lookup-table-modal.component';
import { StoreContactMapContactLookupTableModalComponent } from './storeContactMap-contact-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreContactMapsComponent,
        CreateOrEditStoreContactMapModalComponent,
        ViewStoreContactMapModalComponent,

        StoreContactMapStoreLookupTableModalComponent,
        StoreContactMapContactLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreContactMapRoutingModule, AdminSharedModule],
})
export class StoreContactMapModule {}
