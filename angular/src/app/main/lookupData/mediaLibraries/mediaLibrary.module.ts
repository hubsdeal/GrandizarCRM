import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { MediaLibraryRoutingModule } from './mediaLibrary-routing.module';
import { MediaLibrariesComponent } from './mediaLibraries.component';
import { CreateOrEditMediaLibraryModalComponent } from './create-or-edit-mediaLibrary-modal.component';
import { ViewMediaLibraryModalComponent } from './view-mediaLibrary-modal.component';
import { MediaLibraryMasterTagCategoryLookupTableModalComponent } from './mediaLibrary-masterTagCategory-lookup-table-modal.component';
import { MediaLibraryMasterTagLookupTableModalComponent } from './mediaLibrary-masterTag-lookup-table-modal.component';

@NgModule({
    declarations: [
        MediaLibrariesComponent,
        CreateOrEditMediaLibraryModalComponent,
        ViewMediaLibraryModalComponent,

        MediaLibraryMasterTagCategoryLookupTableModalComponent,
        MediaLibraryMasterTagLookupTableModalComponent,
    ],
    imports: [AppSharedModule, MediaLibraryRoutingModule, AdminSharedModule],
})
export class MediaLibraryModule {}
