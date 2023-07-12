import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubRoutingModule } from './hub-routing.module';
import { HubsComponent } from './hubs.component';
import { CreateOrEditHubModalComponent } from './create-or-edit-hub-modal.component';
import { ViewHubModalComponent } from './view-hub-modal.component';
import { HubMediaLibraryLookupTableModalComponent } from './hub-mediaLibrary-lookup-table-modal.component';
import { HubDashboardComponent } from './hub-dashboard/hub-dashboard.component';
import { MyHubListComponent } from './my-hub-list/my-hub-list.component';
import { StoreModule } from '@app/main/shop/stores/store.module';
import { HubWidgetMapModule } from '@app/main/widgetManagement/hubWidgetMaps/hubWidgetMap.module';

@NgModule({
    declarations: [
        HubsComponent,
        CreateOrEditHubModalComponent,
        ViewHubModalComponent,

        HubMediaLibraryLookupTableModalComponent,
          HubDashboardComponent,
          MyHubListComponent,
    ],
    imports: [AppSharedModule, HubRoutingModule, AdminSharedModule, StoreModule, HubWidgetMapModule],
})
export class HubModule {}
