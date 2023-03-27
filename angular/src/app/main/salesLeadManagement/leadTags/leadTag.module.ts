import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { LeadTagRoutingModule } from './leadTag-routing.module';
import { LeadTagsComponent } from './leadTags.component';
import { CreateOrEditLeadTagModalComponent } from './create-or-edit-leadTag-modal.component';
import { ViewLeadTagModalComponent } from './view-leadTag-modal.component';
import { LeadTagLeadLookupTableModalComponent } from './leadTag-lead-lookup-table-modal.component';
import { LeadTagMasterTagCategoryLookupTableModalComponent } from './leadTag-masterTagCategory-lookup-table-modal.component';
import { LeadTagMasterTagLookupTableModalComponent } from './leadTag-masterTag-lookup-table-modal.component';

@NgModule({
    declarations: [
        LeadTagsComponent,
        CreateOrEditLeadTagModalComponent,
        ViewLeadTagModalComponent,

        LeadTagLeadLookupTableModalComponent,
        LeadTagMasterTagCategoryLookupTableModalComponent,
        LeadTagMasterTagLookupTableModalComponent,
    ],
    imports: [AppSharedModule, LeadTagRoutingModule, AdminSharedModule],
})
export class LeadTagModule {}
