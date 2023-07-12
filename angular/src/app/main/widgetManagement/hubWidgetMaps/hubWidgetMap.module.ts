import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubWidgetMapRoutingModule } from './hubWidgetMap-routing.module';
import { HubWidgetMapsComponent } from './hubWidgetMaps.component';
import { CreateOrEditHubWidgetMapModalComponent } from './create-or-edit-hubWidgetMap-modal.component';
import { ViewHubWidgetMapModalComponent } from './view-hubWidgetMap-modal.component';
import { HubWidgetMapHubLookupTableModalComponent } from './hubWidgetMap-hub-lookup-table-modal.component';
import { HubWidgetMapMasterWidgetLookupTableModalComponent } from './hubWidgetMap-masterWidget-lookup-table-modal.component';

@NgModule({
    declarations: [
        HubWidgetMapsComponent,
        CreateOrEditHubWidgetMapModalComponent,
        ViewHubWidgetMapModalComponent,

        HubWidgetMapHubLookupTableModalComponent,
        HubWidgetMapMasterWidgetLookupTableModalComponent,
    ],
    imports: [AppSharedModule, HubWidgetMapRoutingModule, AdminSharedModule],
    exports: [HubWidgetMapsComponent, CreateOrEditHubWidgetMapModalComponent, ViewHubWidgetMapModalComponent],
})
export class HubWidgetMapModule {}
