import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { LeadPipelineStageRoutingModule } from './leadPipelineStage-routing.module';
import { LeadPipelineStagesComponent } from './leadPipelineStages.component';
import { CreateOrEditLeadPipelineStageModalComponent } from './create-or-edit-leadPipelineStage-modal.component';
import { ViewLeadPipelineStageModalComponent } from './view-leadPipelineStage-modal.component';

@NgModule({
    declarations: [
        LeadPipelineStagesComponent,
        CreateOrEditLeadPipelineStageModalComponent,
        ViewLeadPipelineStageModalComponent,
    ],
    imports: [AppSharedModule, LeadPipelineStageRoutingModule, AdminSharedModule],
})
export class LeadPipelineStageModule {}
