import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BusinessNoteRoutingModule } from './businessNote-routing.module';
import { BusinessNotesComponent } from './businessNotes.component';
import { CreateOrEditBusinessNoteModalComponent } from './create-or-edit-businessNote-modal.component';
import { ViewBusinessNoteModalComponent } from './view-businessNote-modal.component';
import { BusinessNoteBusinessLookupTableModalComponent } from './businessNote-business-lookup-table-modal.component';

@NgModule({
    declarations: [
        BusinessNotesComponent,
        CreateOrEditBusinessNoteModalComponent,
        ViewBusinessNoteModalComponent,

        BusinessNoteBusinessLookupTableModalComponent,
    ],
    imports: [AppSharedModule, BusinessNoteRoutingModule, AdminSharedModule],
})
export class BusinessNoteModule {}
