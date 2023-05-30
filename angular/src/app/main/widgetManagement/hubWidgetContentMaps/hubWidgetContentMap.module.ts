import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubWidgetContentMapRoutingModule } from './hubWidgetContentMap-routing.module';
import { HubWidgetContentMapsComponent } from './hubWidgetContentMaps.component';
import { CreateOrEditHubWidgetContentMapModalComponent } from './create-or-edit-hubWidgetContentMap-modal.component';
import { ViewHubWidgetContentMapModalComponent } from './view-hubWidgetContentMap-modal.component';
import { HubWidgetContentMapHubWidgetMapLookupTableModalComponent } from './hubWidgetContentMap-hubWidgetMap-lookup-table-modal.component';
import { HubWidgetContentMapContentLookupTableModalComponent } from './hubWidgetContentMap-content-lookup-table-modal.component';

@NgModule({
    declarations: [
        HubWidgetContentMapsComponent,
        CreateOrEditHubWidgetContentMapModalComponent,
        ViewHubWidgetContentMapModalComponent,

        HubWidgetContentMapHubWidgetMapLookupTableModalComponent,
        HubWidgetContentMapContentLookupTableModalComponent,
    ],
    imports: [AppSharedModule, HubWidgetContentMapRoutingModule, AdminSharedModule],
})
export class HubWidgetContentMapModule {}
