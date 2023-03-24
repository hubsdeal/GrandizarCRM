import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TaskEventRoutingModule } from './taskEvent-routing.module';
import { TaskEventsComponent } from './taskEvents.component';
import { CreateOrEditTaskEventModalComponent } from './create-or-edit-taskEvent-modal.component';
import { ViewTaskEventModalComponent } from './view-taskEvent-modal.component';

@NgModule({
    declarations: [TaskEventsComponent, CreateOrEditTaskEventModalComponent, ViewTaskEventModalComponent],
    imports: [AppSharedModule, TaskEventRoutingModule, AdminSharedModule],
})
export class TaskEventModule {}
