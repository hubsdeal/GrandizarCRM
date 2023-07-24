import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubStoreRoutingModule } from './hubStore-routing.module';
import { HubStoresComponent } from './hubStores.component';
import { CreateOrEditHubStoreModalComponent } from './create-or-edit-hubStore-modal.component';
import { ViewHubStoreModalComponent } from './view-hubStore-modal.component';
import { HubStoreHubLookupTableModalComponent } from './hubStore-hub-lookup-table-modal.component';
import { HubStoreStoreLookupTableModalComponent } from './hubStore-store-lookup-table-modal.component';

@NgModule({
    declarations: [
        HubStoresComponent,
        CreateOrEditHubStoreModalComponent,
        ViewHubStoreModalComponent,

        HubStoreHubLookupTableModalComponent,
        HubStoreStoreLookupTableModalComponent,
    ],
    imports: [AppSharedModule, HubStoreRoutingModule, AdminSharedModule],
    exports: [HubStoresComponent, CreateOrEditHubStoreModalComponent, ViewHubStoreModalComponent],
})
export class HubStoreModule {}
