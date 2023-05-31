import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreWidgetMapRoutingModule } from './storeWidgetMap-routing.module';
import { StoreWidgetMapsComponent } from './storeWidgetMaps.component';
import { CreateOrEditStoreWidgetMapModalComponent } from './create-or-edit-storeWidgetMap-modal.component';
import { ViewStoreWidgetMapModalComponent } from './view-storeWidgetMap-modal.component';
import { StoreWidgetMapMasterWidgetLookupTableModalComponent } from './storeWidgetMap-masterWidget-lookup-table-modal.component';
import { StoreWidgetMapStoreLookupTableModalComponent } from './storeWidgetMap-store-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreWidgetMapsComponent,
        CreateOrEditStoreWidgetMapModalComponent,
        ViewStoreWidgetMapModalComponent,

        StoreWidgetMapMasterWidgetLookupTableModalComponent,
        StoreWidgetMapStoreLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreWidgetMapRoutingModule, AdminSharedModule],
})
export class StoreWidgetMapModule {}
