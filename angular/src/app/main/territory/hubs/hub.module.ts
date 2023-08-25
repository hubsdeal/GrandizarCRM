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
import { HubZipCodeMapModule } from '../hubZipCodeMaps/hubZipCodeMap.module';
import { HubNavigationMenuModule } from '../hubNavigationMenus/hubNavigationMenu.module';
import { HubSalesProjectionModule } from '../hubSalesProjections/hubSalesProjection.module';
import { HubAccountTeamModule } from '../hubAccountTeams/hubAccountTeam.module';
import { HubProductCategoryModule } from '../hubProductCategories/hubProductCategory.module';
import { HubProductModule } from '../hubProducts/hubProduct.module';
import { HubStoreModule } from '../hubStores/hubStore.module';
import { HubContactModule } from '../hubContacts/hubContact.module';
import { HubBusinessModule } from '../hubBusinesses/hubBusiness.module';
import { HubDocumentComponent } from './hub-dashboard/hub-document/hub-document.component';

@NgModule({
    declarations: [
        HubsComponent,
        CreateOrEditHubModalComponent,
        ViewHubModalComponent,

        HubMediaLibraryLookupTableModalComponent,
        HubDashboardComponent,
        MyHubListComponent,
        HubDocumentComponent,
    ],
    imports: [AppSharedModule, HubRoutingModule, AdminSharedModule, StoreModule, HubWidgetMapModule, HubZipCodeMapModule, HubNavigationMenuModule,
        HubSalesProjectionModule,
        HubAccountTeamModule,
        HubProductCategoryModule,
        HubProductModule,
        HubStoreModule,
        HubContactModule,
        HubBusinessModule
    ],
})
export class HubModule { }
