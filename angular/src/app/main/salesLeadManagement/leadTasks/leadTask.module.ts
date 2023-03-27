import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { LeadTaskRoutingModule } from './leadTask-routing.module';
import { LeadTasksComponent } from './leadTasks.component';
import { CreateOrEditLeadTaskModalComponent } from './create-or-edit-leadTask-modal.component';
import { ViewLeadTaskModalComponent } from './view-leadTask-modal.component';
import { LeadTaskLeadLookupTableModalComponent } from './leadTask-lead-lookup-table-modal.component';
import { LeadTaskTaskEventLookupTableModalComponent } from './leadTask-taskEvent-lookup-table-modal.component';

@NgModule({
    declarations: [
        LeadTasksComponent,
        CreateOrEditLeadTaskModalComponent,
        ViewLeadTaskModalComponent,

        LeadTaskLeadLookupTableModalComponent,
        LeadTaskTaskEventLookupTableModalComponent,
    ],
    imports: [AppSharedModule, LeadTaskRoutingModule, AdminSharedModule],
})
export class LeadTaskModule {}
