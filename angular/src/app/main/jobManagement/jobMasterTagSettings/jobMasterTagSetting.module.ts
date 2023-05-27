import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { JobMasterTagSettingRoutingModule } from './jobMasterTagSetting-routing.module';
import { JobMasterTagSettingsComponent } from './jobMasterTagSettings.component';
import { CreateOrEditJobMasterTagSettingModalComponent } from './create-or-edit-jobMasterTagSetting-modal.component';
import { ViewJobMasterTagSettingModalComponent } from './view-jobMasterTagSetting-modal.component';
import { JobMasterTagSettingMasterTagLookupTableModalComponent } from './jobMasterTagSetting-masterTag-lookup-table-modal.component';
import { JobMasterTagSettingMasterTagCategoryLookupTableModalComponent } from './jobMasterTagSetting-masterTagCategory-lookup-table-modal.component';

@NgModule({
    declarations: [
        JobMasterTagSettingsComponent,
        CreateOrEditJobMasterTagSettingModalComponent,
        ViewJobMasterTagSettingModalComponent,

        JobMasterTagSettingMasterTagLookupTableModalComponent,
        JobMasterTagSettingMasterTagCategoryLookupTableModalComponent,
    ],
    imports: [AppSharedModule, JobMasterTagSettingRoutingModule, AdminSharedModule],
})
export class JobMasterTagSettingModule {}
