import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TaskWorkItemRoutingModule } from './taskWorkItem-routing.module';
import { TaskWorkItemsComponent } from './taskWorkItems.component';
import { CreateOrEditTaskWorkItemModalComponent } from './create-or-edit-taskWorkItem-modal.component';
import { ViewTaskWorkItemModalComponent } from './view-taskWorkItem-modal.component';
import { TaskWorkItemTaskEventLookupTableModalComponent } from './taskWorkItem-taskEvent-lookup-table-modal.component';
import { TaskWorkItemEmployeeLookupTableModalComponent } from './taskWorkItem-employee-lookup-table-modal.component';

@NgModule({
    declarations: [
        TaskWorkItemsComponent,
        CreateOrEditTaskWorkItemModalComponent,
        ViewTaskWorkItemModalComponent,

        TaskWorkItemTaskEventLookupTableModalComponent,
        TaskWorkItemEmployeeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, TaskWorkItemRoutingModule, AdminSharedModule],
})
export class TaskWorkItemModule {}
