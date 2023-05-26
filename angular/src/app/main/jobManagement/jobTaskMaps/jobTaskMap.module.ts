import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { JobTaskMapRoutingModule } from './jobTaskMap-routing.module';
import { JobTaskMapsComponent } from './jobTaskMaps.component';
import { CreateOrEditJobTaskMapModalComponent } from './create-or-edit-jobTaskMap-modal.component';
import { ViewJobTaskMapModalComponent } from './view-jobTaskMap-modal.component';
import { JobTaskMapJobLookupTableModalComponent } from './jobTaskMap-job-lookup-table-modal.component';
import { JobTaskMapTaskEventLookupTableModalComponent } from './jobTaskMap-taskEvent-lookup-table-modal.component';

@NgModule({
    declarations: [
        JobTaskMapsComponent,
        CreateOrEditJobTaskMapModalComponent,
        ViewJobTaskMapModalComponent,

        JobTaskMapJobLookupTableModalComponent,
        JobTaskMapTaskEventLookupTableModalComponent,
    ],
    imports: [AppSharedModule, JobTaskMapRoutingModule, AdminSharedModule],
})
export class JobTaskMapModule {}
