import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreZipCodeMapRoutingModule } from './storeZipCodeMap-routing.module';
import { StoreZipCodeMapsComponent } from './storeZipCodeMaps.component';
import { CreateOrEditStoreZipCodeMapModalComponent } from './create-or-edit-storeZipCodeMap-modal.component';
import { ViewStoreZipCodeMapModalComponent } from './view-storeZipCodeMap-modal.component';
import { StoreZipCodeMapStoreLookupTableModalComponent } from './storeZipCodeMap-store-lookup-table-modal.component';
import { StoreZipCodeMapZipCodeLookupTableModalComponent } from './storeZipCodeMap-zipCode-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreZipCodeMapsComponent,
        CreateOrEditStoreZipCodeMapModalComponent,
        ViewStoreZipCodeMapModalComponent,

        StoreZipCodeMapStoreLookupTableModalComponent,
        StoreZipCodeMapZipCodeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreZipCodeMapRoutingModule, AdminSharedModule],
})
export class StoreZipCodeMapModule {}
