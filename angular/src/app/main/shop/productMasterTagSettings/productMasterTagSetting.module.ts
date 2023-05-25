import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductMasterTagSettingRoutingModule } from './productMasterTagSetting-routing.module';
import { ProductMasterTagSettingsComponent } from './productMasterTagSettings.component';
import { CreateOrEditProductMasterTagSettingModalComponent } from './create-or-edit-productMasterTagSetting-modal.component';
import { ViewProductMasterTagSettingModalComponent } from './view-productMasterTagSetting-modal.component';
import { ProductMasterTagSettingProductCategoryLookupTableModalComponent } from './productMasterTagSetting-productCategory-lookup-table-modal.component';
import { ProductMasterTagSettingMasterTagCategoryLookupTableModalComponent } from './productMasterTagSetting-masterTagCategory-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductMasterTagSettingsComponent,
        CreateOrEditProductMasterTagSettingModalComponent,
        ViewProductMasterTagSettingModalComponent,

        ProductMasterTagSettingProductCategoryLookupTableModalComponent,
        ProductMasterTagSettingMasterTagCategoryLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductMasterTagSettingRoutingModule, AdminSharedModule],
})
export class ProductMasterTagSettingModule {}
