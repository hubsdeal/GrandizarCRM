import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreWidgetContentMapRoutingModule } from './storeWidgetContentMap-routing.module';
import { StoreWidgetContentMapsComponent } from './storeWidgetContentMaps.component';
import { CreateOrEditStoreWidgetContentMapModalComponent } from './create-or-edit-storeWidgetContentMap-modal.component';
import { ViewStoreWidgetContentMapModalComponent } from './view-storeWidgetContentMap-modal.component';
import { StoreWidgetContentMapStoreWidgetMapLookupTableModalComponent } from './storeWidgetContentMap-storeWidgetMap-lookup-table-modal.component';
import { StoreWidgetContentMapContentLookupTableModalComponent } from './storeWidgetContentMap-content-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreWidgetContentMapsComponent,
        CreateOrEditStoreWidgetContentMapModalComponent,
        ViewStoreWidgetContentMapModalComponent,

        StoreWidgetContentMapStoreWidgetMapLookupTableModalComponent,
        StoreWidgetContentMapContentLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreWidgetContentMapRoutingModule, AdminSharedModule],
})
export class StoreWidgetContentMapModule {}
