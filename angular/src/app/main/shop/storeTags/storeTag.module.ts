import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreTagRoutingModule } from './storeTag-routing.module';
import { StoreTagsComponent } from './storeTags.component';
import { CreateOrEditStoreTagModalComponent } from './create-or-edit-storeTag-modal.component';
import { ViewStoreTagModalComponent } from './view-storeTag-modal.component';
import { StoreTagStoreLookupTableModalComponent } from './storeTag-store-lookup-table-modal.component';
import { StoreTagMasterTagCategoryLookupTableModalComponent } from './storeTag-masterTagCategory-lookup-table-modal.component';
import { StoreTagMasterTagLookupTableModalComponent } from './storeTag-masterTag-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreTagsComponent,
        CreateOrEditStoreTagModalComponent,
        ViewStoreTagModalComponent,

        StoreTagStoreLookupTableModalComponent,
        StoreTagMasterTagCategoryLookupTableModalComponent,
        StoreTagMasterTagLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreTagRoutingModule, AdminSharedModule],
})
export class StoreTagModule {}
