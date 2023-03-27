import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreRelevantStoreRoutingModule } from './storeRelevantStore-routing.module';
import { StoreRelevantStoresComponent } from './storeRelevantStores.component';
import { CreateOrEditStoreRelevantStoreModalComponent } from './create-or-edit-storeRelevantStore-modal.component';
import { ViewStoreRelevantStoreModalComponent } from './view-storeRelevantStore-modal.component';
import { StoreRelevantStoreStoreLookupTableModalComponent } from './storeRelevantStore-store-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreRelevantStoresComponent,
        CreateOrEditStoreRelevantStoreModalComponent,
        ViewStoreRelevantStoreModalComponent,

        StoreRelevantStoreStoreLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreRelevantStoreRoutingModule, AdminSharedModule],
})
export class StoreRelevantStoreModule {}
