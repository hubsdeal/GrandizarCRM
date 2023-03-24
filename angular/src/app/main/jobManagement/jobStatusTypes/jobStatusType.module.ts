import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { JobStatusTypeRoutingModule } from './jobStatusType-routing.module';
import { JobStatusTypesComponent } from './jobStatusTypes.component';
import { CreateOrEditJobStatusTypeModalComponent } from './create-or-edit-jobStatusType-modal.component';
import { ViewJobStatusTypeModalComponent } from './view-jobStatusType-modal.component';

@NgModule({
    declarations: [JobStatusTypesComponent, CreateOrEditJobStatusTypeModalComponent, ViewJobStatusTypeModalComponent],
    imports: [AppSharedModule, JobStatusTypeRoutingModule, AdminSharedModule],
})
export class JobStatusTypeModule {}
