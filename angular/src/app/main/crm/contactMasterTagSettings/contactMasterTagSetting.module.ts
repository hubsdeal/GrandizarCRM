import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ContactMasterTagSettingRoutingModule } from './contactMasterTagSetting-routing.module';
import { ContactMasterTagSettingsComponent } from './contactMasterTagSettings.component';
import { CreateOrEditContactMasterTagSettingModalComponent } from './create-or-edit-contactMasterTagSetting-modal.component';
import { ViewContactMasterTagSettingModalComponent } from './view-contactMasterTagSetting-modal.component';
import { ContactMasterTagSettingMasterTagLookupTableModalComponent } from './contactMasterTagSetting-masterTag-lookup-table-modal.component';
import { ContactMasterTagSettingMasterTagCategoryLookupTableModalComponent } from './contactMasterTagSetting-masterTagCategory-lookup-table-modal.component';

@NgModule({
    declarations: [
        ContactMasterTagSettingsComponent,
        CreateOrEditContactMasterTagSettingModalComponent,
        ViewContactMasterTagSettingModalComponent,

        ContactMasterTagSettingMasterTagLookupTableModalComponent,
        ContactMasterTagSettingMasterTagCategoryLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ContactMasterTagSettingRoutingModule, AdminSharedModule],
})
export class ContactMasterTagSettingModule {}
