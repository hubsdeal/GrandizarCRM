import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { MasterTagCategoryRoutingModule } from './masterTagCategory-routing.module';
import { MasterTagCategoriesComponent } from './masterTagCategories.component';
import { CreateOrEditMasterTagCategoryModalComponent } from './create-or-edit-masterTagCategory-modal.component';
import { ViewMasterTagCategoryModalComponent } from './view-masterTagCategory-modal.component';

@NgModule({
    declarations: [
        MasterTagCategoriesComponent,
        CreateOrEditMasterTagCategoryModalComponent,
        ViewMasterTagCategoryModalComponent,
    ],
    imports: [AppSharedModule, MasterTagCategoryRoutingModule, AdminSharedModule],
})
export class MasterTagCategoryModule {}
