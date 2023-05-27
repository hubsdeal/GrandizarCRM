import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BusinessMasterTagSettingRoutingModule } from './businessMasterTagSetting-routing.module';
import { BusinessMasterTagSettingsComponent } from './businessMasterTagSettings.component';
import { CreateOrEditBusinessMasterTagSettingModalComponent } from './create-or-edit-businessMasterTagSetting-modal.component';
import { ViewBusinessMasterTagSettingModalComponent } from './view-businessMasterTagSetting-modal.component';
import { BusinessMasterTagSettingMasterTagCategoryLookupTableModalComponent } from './businessMasterTagSetting-masterTagCategory-lookup-table-modal.component';
import { BusinessMasterTagSettingMasterTagLookupTableModalComponent } from './businessMasterTagSetting-masterTag-lookup-table-modal.component';

@NgModule({
    declarations: [
        BusinessMasterTagSettingsComponent,
        CreateOrEditBusinessMasterTagSettingModalComponent,
        ViewBusinessMasterTagSettingModalComponent,

        BusinessMasterTagSettingMasterTagCategoryLookupTableModalComponent,
        BusinessMasterTagSettingMasterTagLookupTableModalComponent,
    ],
    imports: [AppSharedModule, BusinessMasterTagSettingRoutingModule, AdminSharedModule],
})
export class BusinessMasterTagSettingModule {}
