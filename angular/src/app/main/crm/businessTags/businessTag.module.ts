import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BusinessTagRoutingModule } from './businessTag-routing.module';
import { BusinessTagsComponent } from './businessTags.component';
import { CreateOrEditBusinessTagModalComponent } from './create-or-edit-businessTag-modal.component';
import { ViewBusinessTagModalComponent } from './view-businessTag-modal.component';
import { BusinessTagBusinessLookupTableModalComponent } from './businessTag-business-lookup-table-modal.component';
import { BusinessTagMasterTagCategoryLookupTableModalComponent } from './businessTag-masterTagCategory-lookup-table-modal.component';
import { BusinessTagMasterTagLookupTableModalComponent } from './businessTag-masterTag-lookup-table-modal.component';

@NgModule({
    declarations: [
        BusinessTagsComponent,
        CreateOrEditBusinessTagModalComponent,
        ViewBusinessTagModalComponent,

        BusinessTagBusinessLookupTableModalComponent,
        BusinessTagMasterTagCategoryLookupTableModalComponent,
        BusinessTagMasterTagLookupTableModalComponent,
    ],
    imports: [AppSharedModule, BusinessTagRoutingModule, AdminSharedModule],
})
export class BusinessTagModule {}
