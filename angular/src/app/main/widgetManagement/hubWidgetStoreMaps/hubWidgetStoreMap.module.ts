import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubWidgetStoreMapRoutingModule } from './hubWidgetStoreMap-routing.module';
import { HubWidgetStoreMapsComponent } from './hubWidgetStoreMaps.component';
import { CreateOrEditHubWidgetStoreMapModalComponent } from './create-or-edit-hubWidgetStoreMap-modal.component';
import { ViewHubWidgetStoreMapModalComponent } from './view-hubWidgetStoreMap-modal.component';
import { HubWidgetStoreMapHubWidgetMapLookupTableModalComponent } from './hubWidgetStoreMap-hubWidgetMap-lookup-table-modal.component';
import { HubWidgetStoreMapStoreLookupTableModalComponent } from './hubWidgetStoreMap-store-lookup-table-modal.component';

@NgModule({
    declarations: [
        HubWidgetStoreMapsComponent,
        CreateOrEditHubWidgetStoreMapModalComponent,
        ViewHubWidgetStoreMapModalComponent,

        HubWidgetStoreMapHubWidgetMapLookupTableModalComponent,
        HubWidgetStoreMapStoreLookupTableModalComponent,
    ],
    imports: [AppSharedModule, HubWidgetStoreMapRoutingModule, AdminSharedModule],
})
export class HubWidgetStoreMapModule {}
