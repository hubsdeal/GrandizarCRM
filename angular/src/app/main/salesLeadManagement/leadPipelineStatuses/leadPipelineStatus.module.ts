import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { LeadPipelineStatusRoutingModule } from './leadPipelineStatus-routing.module';
import { LeadPipelineStatusesComponent } from './leadPipelineStatuses.component';
import { CreateOrEditLeadPipelineStatusModalComponent } from './create-or-edit-leadPipelineStatus-modal.component';
import { ViewLeadPipelineStatusModalComponent } from './view-leadPipelineStatus-modal.component';
import { LeadPipelineStatusLeadLookupTableModalComponent } from './leadPipelineStatus-lead-lookup-table-modal.component';
import { LeadPipelineStatusLeadPipelineStageLookupTableModalComponent } from './leadPipelineStatus-leadPipelineStage-lookup-table-modal.component';
import { LeadPipelineStatusEmployeeLookupTableModalComponent } from './leadPipelineStatus-employee-lookup-table-modal.component';

@NgModule({
    declarations: [
        LeadPipelineStatusesComponent,
        CreateOrEditLeadPipelineStatusModalComponent,
        ViewLeadPipelineStatusModalComponent,

        LeadPipelineStatusLeadLookupTableModalComponent,
        LeadPipelineStatusLeadPipelineStageLookupTableModalComponent,
        LeadPipelineStatusEmployeeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, LeadPipelineStatusRoutingModule, AdminSharedModule],
})
export class LeadPipelineStatusModule {}
