import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubContactRoutingModule } from './hubContact-routing.module';
import { HubContactsComponent } from './hubContacts.component';
import { CreateOrEditHubContactModalComponent } from './create-or-edit-hubContact-modal.component';
import { ViewHubContactModalComponent } from './view-hubContact-modal.component';
import { HubContactHubLookupTableModalComponent } from './hubContact-hub-lookup-table-modal.component';
import { HubContactContactLookupTableModalComponent } from './hubContact-contact-lookup-table-modal.component';

@NgModule({
    declarations: [
        HubContactsComponent,
        CreateOrEditHubContactModalComponent,
        ViewHubContactModalComponent,

        HubContactHubLookupTableModalComponent,
        HubContactContactLookupTableModalComponent,
    ],
    imports: [AppSharedModule, HubContactRoutingModule, AdminSharedModule],
})
export class HubContactModule {}
