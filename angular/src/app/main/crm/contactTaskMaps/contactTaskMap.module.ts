import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ContactTaskMapRoutingModule } from './contactTaskMap-routing.module';
import { ContactTaskMapsComponent } from './contactTaskMaps.component';
import { CreateOrEditContactTaskMapModalComponent } from './create-or-edit-contactTaskMap-modal.component';
import { ViewContactTaskMapModalComponent } from './view-contactTaskMap-modal.component';
import { ContactTaskMapContactLookupTableModalComponent } from './contactTaskMap-contact-lookup-table-modal.component';
import { ContactTaskMapTaskEventLookupTableModalComponent } from './contactTaskMap-taskEvent-lookup-table-modal.component';

@NgModule({
    declarations: [
        ContactTaskMapsComponent,
        CreateOrEditContactTaskMapModalComponent,
        ViewContactTaskMapModalComponent,

        ContactTaskMapContactLookupTableModalComponent,
        ContactTaskMapTaskEventLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ContactTaskMapRoutingModule, AdminSharedModule],
})
export class ContactTaskMapModule {}
