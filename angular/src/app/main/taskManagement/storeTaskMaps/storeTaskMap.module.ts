import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreTaskMapRoutingModule } from './storeTaskMap-routing.module';
import { StoreTaskMapsComponent } from './storeTaskMaps.component';
import { CreateOrEditStoreTaskMapModalComponent } from './create-or-edit-storeTaskMap-modal.component';
import { ViewStoreTaskMapModalComponent } from './view-storeTaskMap-modal.component';
import { StoreTaskMapStoreLookupTableModalComponent } from './storeTaskMap-store-lookup-table-modal.component';
import { StoreTaskMapTaskEventLookupTableModalComponent } from './storeTaskMap-taskEvent-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreTaskMapsComponent,
        CreateOrEditStoreTaskMapModalComponent,
        ViewStoreTaskMapModalComponent,

        StoreTaskMapStoreLookupTableModalComponent,
        StoreTaskMapTaskEventLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreTaskMapRoutingModule, AdminSharedModule],
    exports: [
        StoreTaskMapsComponent,
        CreateOrEditStoreTaskMapModalComponent,
        ViewStoreTaskMapModalComponent,

        StoreTaskMapStoreLookupTableModalComponent,
        StoreTaskMapTaskEventLookupTableModalComponent,
    ]
})
export class StoreTaskMapModule {}
