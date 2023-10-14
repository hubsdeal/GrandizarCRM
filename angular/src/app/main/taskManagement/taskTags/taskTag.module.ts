import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TaskTagRoutingModule } from './taskTag-routing.module';
import { TaskTagsComponent } from './taskTags.component';
import { CreateOrEditTaskTagModalComponent } from './create-or-edit-taskTag-modal.component';
import { ViewTaskTagModalComponent } from './view-taskTag-modal.component';
import { TaskTagTaskEventLookupTableModalComponent } from './taskTag-taskEvent-lookup-table-modal.component';
import { TaskTagMasterTagCategoryLookupTableModalComponent } from './taskTag-masterTagCategory-lookup-table-modal.component';
import { TaskTagMasterTagLookupTableModalComponent } from './taskTag-masterTag-lookup-table-modal.component';

@NgModule({
    declarations: [
        TaskTagsComponent,
        CreateOrEditTaskTagModalComponent,
        ViewTaskTagModalComponent,

        TaskTagTaskEventLookupTableModalComponent,
        TaskTagMasterTagCategoryLookupTableModalComponent,
        TaskTagMasterTagLookupTableModalComponent,
    ],
    imports: [AppSharedModule, TaskTagRoutingModule, AdminSharedModule],
    exports:[TaskTagsComponent,
        CreateOrEditTaskTagModalComponent,
        ViewTaskTagModalComponent,

        TaskTagTaskEventLookupTableModalComponent,
        TaskTagMasterTagCategoryLookupTableModalComponent,
        TaskTagMasterTagLookupTableModalComponent,]
})
export class TaskTagModule {}
