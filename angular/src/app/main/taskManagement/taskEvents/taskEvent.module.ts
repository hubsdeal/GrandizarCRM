import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TaskEventRoutingModule } from './taskEvent-routing.module';
import { TaskEventsComponent } from './taskEvents.component';
import { CreateOrEditTaskEventModalComponent } from './create-or-edit-taskEvent-modal.component';
import { ViewTaskEventModalComponent } from './view-taskEvent-modal.component';
import { TaskEventsDashboardComponent } from './task-events-dashboard/task-events-dashboard.component';
import { MyTaskEventsComponent } from './my-task-events/my-task-events.component';
import { TaskEventsLibraryComponent } from './task-events-library/task-events-library.component';
import { TaskWorkItemModule } from '../taskWorkItems/taskWorkItem.module';
import { TaskEventsLookupTableModalComponent } from './task-events-lookup-table-modal/task-events-lookup-table-modal.component';

@NgModule({
    declarations: [TaskEventsComponent, CreateOrEditTaskEventModalComponent, ViewTaskEventModalComponent, TaskEventsDashboardComponent, MyTaskEventsComponent, TaskEventsLibraryComponent, TaskEventsLookupTableModalComponent],
    imports: [AppSharedModule, TaskEventRoutingModule, AdminSharedModule, TaskWorkItemModule],
    exports: [TaskEventsComponent, CreateOrEditTaskEventModalComponent, ViewTaskEventModalComponent, TaskEventsDashboardComponent, MyTaskEventsComponent],
})
export class TaskEventModule {}
