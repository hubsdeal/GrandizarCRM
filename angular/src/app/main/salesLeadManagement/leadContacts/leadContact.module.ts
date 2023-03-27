import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { LeadContactRoutingModule } from './leadContact-routing.module';
import { LeadContactsComponent } from './leadContacts.component';
import { CreateOrEditLeadContactModalComponent } from './create-or-edit-leadContact-modal.component';
import { ViewLeadContactModalComponent } from './view-leadContact-modal.component';
import { LeadContactLeadLookupTableModalComponent } from './leadContact-lead-lookup-table-modal.component';
import { LeadContactContactLookupTableModalComponent } from './leadContact-contact-lookup-table-modal.component';

@NgModule({
    declarations: [
        LeadContactsComponent,
        CreateOrEditLeadContactModalComponent,
        ViewLeadContactModalComponent,

        LeadContactLeadLookupTableModalComponent,
        LeadContactContactLookupTableModalComponent,
    ],
    imports: [AppSharedModule, LeadContactRoutingModule, AdminSharedModule],
})
export class LeadContactModule {}
