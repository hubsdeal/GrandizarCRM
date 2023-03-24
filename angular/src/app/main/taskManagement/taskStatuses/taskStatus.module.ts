import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TaskStatusRoutingModule } from './taskStatus-routing.module';
import { TaskStatusesComponent } from './taskStatuses.component';
import { CreateOrEditTaskStatusModalComponent } from './create-or-edit-taskStatus-modal.component';
import { ViewTaskStatusModalComponent } from './view-taskStatus-modal.component';

@NgModule({
    declarations: [TaskStatusesComponent, CreateOrEditTaskStatusModalComponent, ViewTaskStatusModalComponent],
    imports: [AppSharedModule, TaskStatusRoutingModule, AdminSharedModule],
})
export class TaskStatusModule {}
