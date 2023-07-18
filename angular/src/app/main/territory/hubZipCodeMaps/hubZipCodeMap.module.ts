import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubZipCodeMapRoutingModule } from './hubZipCodeMap-routing.module';
import { HubZipCodeMapsComponent } from './hubZipCodeMaps.component';
import { CreateOrEditHubZipCodeMapModalComponent } from './create-or-edit-hubZipCodeMap-modal.component';
import { ViewHubZipCodeMapModalComponent } from './view-hubZipCodeMap-modal.component';
import { HubZipCodeMapHubLookupTableModalComponent } from './hubZipCodeMap-hub-lookup-table-modal.component';
import { HubZipCodeMapCityLookupTableModalComponent } from './hubZipCodeMap-city-lookup-table-modal.component';
import { HubZipCodeMapZipCodeLookupTableModalComponent } from './hubZipCodeMap-zipCode-lookup-table-modal.component';

@NgModule({
    declarations: [
        HubZipCodeMapsComponent,
        CreateOrEditHubZipCodeMapModalComponent,
        ViewHubZipCodeMapModalComponent,

        HubZipCodeMapHubLookupTableModalComponent,
        HubZipCodeMapCityLookupTableModalComponent,
        HubZipCodeMapZipCodeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, HubZipCodeMapRoutingModule, AdminSharedModule],
    exports: [HubZipCodeMapsComponent, CreateOrEditHubZipCodeMapModalComponent, ViewHubZipCodeMapModalComponent],
})
export class HubZipCodeMapModule {}
