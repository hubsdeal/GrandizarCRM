import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreWidgetProductMapRoutingModule } from './storeWidgetProductMap-routing.module';
import { StoreWidgetProductMapsComponent } from './storeWidgetProductMaps.component';
import { CreateOrEditStoreWidgetProductMapModalComponent } from './create-or-edit-storeWidgetProductMap-modal.component';
import { ViewStoreWidgetProductMapModalComponent } from './view-storeWidgetProductMap-modal.component';
import { StoreWidgetProductMapStoreWidgetMapLookupTableModalComponent } from './storeWidgetProductMap-storeWidgetMap-lookup-table-modal.component';
import { StoreWidgetProductMapProductLookupTableModalComponent } from './storeWidgetProductMap-product-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreWidgetProductMapsComponent,
        CreateOrEditStoreWidgetProductMapModalComponent,
        ViewStoreWidgetProductMapModalComponent,

        StoreWidgetProductMapStoreWidgetMapLookupTableModalComponent,
        StoreWidgetProductMapProductLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreWidgetProductMapRoutingModule, AdminSharedModule],
})
export class StoreWidgetProductMapModule {}
