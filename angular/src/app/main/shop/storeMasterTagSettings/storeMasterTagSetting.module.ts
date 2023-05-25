import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreMasterTagSettingRoutingModule } from './storeMasterTagSetting-routing.module';
import { StoreMasterTagSettingsComponent } from './storeMasterTagSettings.component';
import { CreateOrEditStoreMasterTagSettingModalComponent } from './create-or-edit-storeMasterTagSetting-modal.component';
import { ViewStoreMasterTagSettingModalComponent } from './view-storeMasterTagSetting-modal.component';
import { StoreMasterTagSettingStoreTagSettingCategoryLookupTableModalComponent } from './storeMasterTagSetting-storeTagSettingCategory-lookup-table-modal.component';
import { StoreMasterTagSettingMasterTagCategoryLookupTableModalComponent } from './storeMasterTagSetting-masterTagCategory-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreMasterTagSettingsComponent,
        CreateOrEditStoreMasterTagSettingModalComponent,
        ViewStoreMasterTagSettingModalComponent,

        StoreMasterTagSettingStoreTagSettingCategoryLookupTableModalComponent,
        StoreMasterTagSettingMasterTagCategoryLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreMasterTagSettingRoutingModule, AdminSharedModule],
})
export class StoreMasterTagSettingModule {}
