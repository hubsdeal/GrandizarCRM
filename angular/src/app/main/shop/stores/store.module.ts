import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreRoutingModule } from './store-routing.module';
import { StoresComponent } from './stores.component';
import { CreateOrEditStoreModalComponent } from './create-or-edit-store-modal.component';
import { ViewStoreModalComponent } from './view-store-modal.component';
import { StoreMediaLibraryLookupTableModalComponent } from './store-mediaLibrary-lookup-table-modal.component';
import { StoreMasterTagLookupTableModalComponent } from './store-masterTag-lookup-table-modal.component';
import { StoreDashboardComponent } from './store-dashboard/store-dashboard.component';
import { StoreMediaModule } from '../storeMedias/storeMedia.module';
import { MyStoresComponent } from './my-stores/my-stores.component';
import { TaskEventModule } from '@app/main/taskManagement/taskEvents/taskEvent.module';
import { StoreTagModule } from '../storeTags/storeTag.module';
import { StoreNoteModule } from '../storeNotes/storeNote.module';
import { OrderModule } from '@app/main/orderManagement/orders/order.module';
import { StoreProductMapModule } from '../storeProductMaps/storeProductMap.module';
import { StoreContactMapModule } from '../storeContactMaps/storeContactMap.module';
import { StoreMarketplaceCommissionSettingModule } from '../storeMarketplaceCommissionSettings/storeMarketplaceCommissionSetting.module';
import { StoreTaskMapModule } from '@app/main/taskManagement/storeTaskMaps/storeTaskMap.module';
import { StoreAccountTeamModule } from '../storeAccountTeams/storeAccountTeam.module';
import { MainModule } from '@app/main/main.module';
import { StoreBusinessHourModule } from '../storeBusinessHours/storeBusinessHour.module';
import { StoreOwnerTeamModule } from '../storeOwnerTeams/storeOwnerTeam.module';

@NgModule({
    declarations: [
        StoresComponent,
        CreateOrEditStoreModalComponent,
        ViewStoreModalComponent,

        StoreMediaLibraryLookupTableModalComponent,
        StoreMasterTagLookupTableModalComponent,
        StoreDashboardComponent,
        MyStoresComponent,
    ],
    imports: [AppSharedModule, StoreRoutingModule, AdminSharedModule, StoreMediaModule, TaskEventModule, StoreTagModule, StoreNoteModule,
        OrderModule,
        StoreProductMapModule,
        StoreContactMapModule,
        StoreMarketplaceCommissionSettingModule,
        StoreTaskMapModule,
        StoreAccountTeamModule,
        StoreBusinessHourModule,
        StoreOwnerTeamModule,
        MainModule
    ],
    exports: [
        MyStoresComponent,
        CreateOrEditStoreModalComponent,
        StoreMediaLibraryLookupTableModalComponent,
        StoreMasterTagLookupTableModalComponent,
    ]
})
export class StoreModule { }
