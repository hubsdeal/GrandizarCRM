import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreTagSettingCategoryRoutingModule } from './storeTagSettingCategory-routing.module';
import { StoreTagSettingCategoriesComponent } from './storeTagSettingCategories.component';
import { CreateOrEditStoreTagSettingCategoryModalComponent } from './create-or-edit-storeTagSettingCategory-modal.component';
import { ViewStoreTagSettingCategoryModalComponent } from './view-storeTagSettingCategory-modal.component';

@NgModule({
    declarations: [
        StoreTagSettingCategoriesComponent,
        CreateOrEditStoreTagSettingCategoryModalComponent,
        ViewStoreTagSettingCategoryModalComponent,
    ],
    imports: [AppSharedModule, StoreTagSettingCategoryRoutingModule, AdminSharedModule],
})
export class StoreTagSettingCategoryModule {}
