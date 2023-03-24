import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreMediaRoutingModule } from './storeMedia-routing.module';
import { StoreMediasComponent } from './storeMedias.component';
import { CreateOrEditStoreMediaModalComponent } from './create-or-edit-storeMedia-modal.component';
import { ViewStoreMediaModalComponent } from './view-storeMedia-modal.component';
import { StoreMediaStoreLookupTableModalComponent } from './storeMedia-store-lookup-table-modal.component';
import { StoreMediaMediaLibraryLookupTableModalComponent } from './storeMedia-mediaLibrary-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreMediasComponent,
        CreateOrEditStoreMediaModalComponent,
        ViewStoreMediaModalComponent,

        StoreMediaStoreLookupTableModalComponent,
        StoreMediaMediaLibraryLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreMediaRoutingModule, AdminSharedModule],
})
export class StoreMediaModule {}
