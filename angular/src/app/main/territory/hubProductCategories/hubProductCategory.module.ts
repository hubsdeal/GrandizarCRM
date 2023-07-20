import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubProductCategoryRoutingModule } from './hubProductCategory-routing.module';
import { HubProductCategoriesComponent } from './hubProductCategories.component';
import { CreateOrEditHubProductCategoryModalComponent } from './create-or-edit-hubProductCategory-modal.component';
import { ViewHubProductCategoryModalComponent } from './view-hubProductCategory-modal.component';
import { HubProductCategoryHubLookupTableModalComponent } from './hubProductCategory-hub-lookup-table-modal.component';
import { HubProductCategoryProductCategoryLookupTableModalComponent } from './hubProductCategory-productCategory-lookup-table-modal.component';

@NgModule({
    declarations: [
        HubProductCategoriesComponent,
        CreateOrEditHubProductCategoryModalComponent,
        ViewHubProductCategoryModalComponent,

        HubProductCategoryHubLookupTableModalComponent,
        HubProductCategoryProductCategoryLookupTableModalComponent,
    ],
    imports: [AppSharedModule, HubProductCategoryRoutingModule, AdminSharedModule],
    exports: [HubProductCategoriesComponent, CreateOrEditHubProductCategoryModalComponent, ViewHubProductCategoryModalComponent],
})
export class HubProductCategoryModule {}
