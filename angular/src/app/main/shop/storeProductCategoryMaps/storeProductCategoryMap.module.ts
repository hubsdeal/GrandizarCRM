import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreProductCategoryMapRoutingModule } from './storeProductCategoryMap-routing.module';
import { StoreProductCategoryMapsComponent } from './storeProductCategoryMaps.component';
import { CreateOrEditStoreProductCategoryMapModalComponent } from './create-or-edit-storeProductCategoryMap-modal.component';
import { ViewStoreProductCategoryMapModalComponent } from './view-storeProductCategoryMap-modal.component';
import { StoreProductCategoryMapStoreLookupTableModalComponent } from './storeProductCategoryMap-store-lookup-table-modal.component';
import { StoreProductCategoryMapProductCategoryLookupTableModalComponent } from './storeProductCategoryMap-productCategory-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreProductCategoryMapsComponent,
        CreateOrEditStoreProductCategoryMapModalComponent,
        ViewStoreProductCategoryMapModalComponent,

        StoreProductCategoryMapStoreLookupTableModalComponent,
        StoreProductCategoryMapProductCategoryLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreProductCategoryMapRoutingModule, AdminSharedModule],
})
export class StoreProductCategoryMapModule {}
