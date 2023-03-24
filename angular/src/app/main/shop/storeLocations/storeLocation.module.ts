import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreLocationRoutingModule } from './storeLocation-routing.module';
import { StoreLocationsComponent } from './storeLocations.component';
import { CreateOrEditStoreLocationModalComponent } from './create-or-edit-storeLocation-modal.component';
import { ViewStoreLocationModalComponent } from './view-storeLocation-modal.component';
import { StoreLocationCityLookupTableModalComponent } from './storeLocation-city-lookup-table-modal.component';
import { StoreLocationStateLookupTableModalComponent } from './storeLocation-state-lookup-table-modal.component';
import { StoreLocationCountryLookupTableModalComponent } from './storeLocation-country-lookup-table-modal.component';
import { StoreLocationStoreLookupTableModalComponent } from './storeLocation-store-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreLocationsComponent,
        CreateOrEditStoreLocationModalComponent,
        ViewStoreLocationModalComponent,

        StoreLocationCityLookupTableModalComponent,
        StoreLocationStateLookupTableModalComponent,
        StoreLocationCountryLookupTableModalComponent,
        StoreLocationStoreLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreLocationRoutingModule, AdminSharedModule],
})
export class StoreLocationModule {}
