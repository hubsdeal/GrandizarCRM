import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { MasterTagRoutingModule } from './masterTag-routing.module';
import { MasterTagsComponent } from './masterTags.component';
import { CreateOrEditMasterTagModalComponent } from './create-or-edit-masterTag-modal.component';
import { ViewMasterTagModalComponent } from './view-masterTag-modal.component';
import { MasterTagMediaLibraryLookupTableModalComponent } from './masterTag-mediaLibrary-lookup-table-modal.component';

@NgModule({
    declarations: [
        MasterTagsComponent,
        CreateOrEditMasterTagModalComponent,
        ViewMasterTagModalComponent,

        MasterTagMediaLibraryLookupTableModalComponent,
    ],
    imports: [AppSharedModule, MasterTagRoutingModule, AdminSharedModule],
})
export class MasterTagModule {}
