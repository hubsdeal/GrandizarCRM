import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ContactRoutingModule } from './contact-routing.module';
import { ContactsComponent } from './contacts.component';
import { CreateOrEditContactModalComponent } from './create-or-edit-contact-modal.component';
import { ViewContactModalComponent } from './view-contact-modal.component';
import { ContactUserLookupTableModalComponent } from './contact-user-lookup-table-modal.component';
import { ContactDashboardComponent } from './contact-dashboard/contact-dashboard.component';

@NgModule({
    declarations: [
        ContactsComponent,
        CreateOrEditContactModalComponent,
        ViewContactModalComponent,

        ContactUserLookupTableModalComponent,
          ContactDashboardComponent,
    ],
    imports: [AppSharedModule, ContactRoutingModule, AdminSharedModule],
    exports: [ContactsComponent, CreateOrEditContactModalComponent, ViewContactModalComponent, ContactUserLookupTableModalComponent],
})
export class ContactModule {}
