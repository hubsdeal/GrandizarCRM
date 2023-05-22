import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TaskEventRoutingModule } from './taskEvent-routing.module';
import { TaskEventsComponent } from './taskEvents.component';
import { CreateOrEditTaskEventModalComponent } from './create-or-edit-taskEvent-modal.component';
import { ViewTaskEventModalComponent } from './view-taskEvent-modal.component';
import { TaskEventsDashboardComponent } from './task-events-dashboard/task-events-dashboard.component';
import { MyTaskEventsComponent } from './my-task-events/my-task-events.component';

@NgModule({
    declarations: [TaskEventsComponent, CreateOrEditTaskEventModalComponent, ViewTaskEventModalComponent, TaskEventsDashboardComponent, MyTaskEventsComponent],
    imports: [AppSharedModule, TaskEventRoutingModule, AdminSharedModule],
    exports: [TaskEventsComponent, CreateOrEditTaskEventModalComponent, ViewTaskEventModalComponent, TaskEventsDashboardComponent, MyTaskEventsComponent],
})
export class TaskEventModule {}
