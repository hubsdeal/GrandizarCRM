import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { LeadNoteRoutingModule } from './leadNote-routing.module';
import { LeadNotesComponent } from './leadNotes.component';
import { CreateOrEditLeadNoteModalComponent } from './create-or-edit-leadNote-modal.component';
import { ViewLeadNoteModalComponent } from './view-leadNote-modal.component';
import { LeadNoteLeadLookupTableModalComponent } from './leadNote-lead-lookup-table-modal.component';

@NgModule({
    declarations: [
        LeadNotesComponent,
        CreateOrEditLeadNoteModalComponent,
        ViewLeadNoteModalComponent,

        LeadNoteLeadLookupTableModalComponent,
    ],
    imports: [AppSharedModule, LeadNoteRoutingModule, AdminSharedModule],
})
export class LeadNoteModule {}
