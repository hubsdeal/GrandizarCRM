import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ContactTagRoutingModule } from './contactTag-routing.module';
import { ContactTagsComponent } from './contactTags.component';
import { CreateOrEditContactTagModalComponent } from './create-or-edit-contactTag-modal.component';
import { ViewContactTagModalComponent } from './view-contactTag-modal.component';
import { ContactTagContactLookupTableModalComponent } from './contactTag-contact-lookup-table-modal.component';
import { ContactTagMasterTagCategoryLookupTableModalComponent } from './contactTag-masterTagCategory-lookup-table-modal.component';
import { ContactTagMasterTagLookupTableModalComponent } from './contactTag-masterTag-lookup-table-modal.component';

@NgModule({
    declarations: [
        ContactTagsComponent,
        CreateOrEditContactTagModalComponent,
        ViewContactTagModalComponent,

        ContactTagContactLookupTableModalComponent,
        ContactTagMasterTagCategoryLookupTableModalComponent,
        ContactTagMasterTagLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ContactTagRoutingModule, AdminSharedModule],
})
export class ContactTagModule {}
