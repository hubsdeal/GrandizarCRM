import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreProductMapRoutingModule } from './storeProductMap-routing.module';
import { StoreProductMapsComponent } from './storeProductMaps.component';
import { CreateOrEditStoreProductMapModalComponent } from './create-or-edit-storeProductMap-modal.component';
import { ViewStoreProductMapModalComponent } from './view-storeProductMap-modal.component';
import { StoreProductMapStoreLookupTableModalComponent } from './storeProductMap-store-lookup-table-modal.component';
import { StoreProductMapProductLookupTableModalComponent } from './storeProductMap-product-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreProductMapsComponent,
        CreateOrEditStoreProductMapModalComponent,
        ViewStoreProductMapModalComponent,

        StoreProductMapStoreLookupTableModalComponent,
        StoreProductMapProductLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreProductMapRoutingModule, AdminSharedModule],
    exports:[
        StoreProductMapsComponent,
        CreateOrEditStoreProductMapModalComponent,
        ViewStoreProductMapModalComponent,

        StoreProductMapStoreLookupTableModalComponent,
        StoreProductMapProductLookupTableModalComponent,
    ]
})
export class StoreProductMapModule {}
