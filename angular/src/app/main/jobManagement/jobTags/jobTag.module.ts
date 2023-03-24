import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { JobTagRoutingModule } from './jobTag-routing.module';
import { JobTagsComponent } from './jobTags.component';
import { CreateOrEditJobTagModalComponent } from './create-or-edit-jobTag-modal.component';
import { ViewJobTagModalComponent } from './view-jobTag-modal.component';
import { JobTagJobLookupTableModalComponent } from './jobTag-job-lookup-table-modal.component';
import { JobTagMasterTagCategoryLookupTableModalComponent } from './jobTag-masterTagCategory-lookup-table-modal.component';
import { JobTagMasterTagLookupTableModalComponent } from './jobTag-masterTag-lookup-table-modal.component';

@NgModule({
    declarations: [
        JobTagsComponent,
        CreateOrEditJobTagModalComponent,
        ViewJobTagModalComponent,

        JobTagJobLookupTableModalComponent,
        JobTagMasterTagCategoryLookupTableModalComponent,
        JobTagMasterTagLookupTableModalComponent,
    ],
    imports: [AppSharedModule, JobTagRoutingModule, AdminSharedModule],
})
export class JobTagModule {}
