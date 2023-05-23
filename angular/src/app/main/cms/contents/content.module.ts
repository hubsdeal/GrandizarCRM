import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ContentRoutingModule } from './content-routing.module';
import { ContentsComponent } from './contents.component';
import { CreateOrEditContentModalComponent } from './create-or-edit-content-modal.component';
import { ViewContentModalComponent } from './view-content-modal.component';
import { ContentMediaLibraryLookupTableModalComponent } from './content-mediaLibrary-lookup-table-modal.component';
import { SiteDefaultContentComponent } from './site-default-content/site-default-content.component';

@NgModule({
    declarations: [
        ContentsComponent,
        CreateOrEditContentModalComponent,
        ViewContentModalComponent,

        ContentMediaLibraryLookupTableModalComponent,
          SiteDefaultContentComponent,
    ],
    imports: [AppSharedModule, ContentRoutingModule, AdminSharedModule],
})
export class ContentModule {}
