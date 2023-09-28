import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {CustomerLocalitiesZipCodeMapRoutingModule} from './customerLocalitiesZipCodeMap-routing.module';
import {CustomerLocalitiesZipCodeMapsComponent} from './customerLocalitiesZipCodeMaps.component';
import {CreateOrEditCustomerLocalitiesZipCodeMapModalComponent} from './create-or-edit-customerLocalitiesZipCodeMap-modal.component';
import {ViewCustomerLocalitiesZipCodeMapModalComponent} from './view-customerLocalitiesZipCodeMap-modal.component';
import {CustomerLocalitiesZipCodeMapContactLookupTableModalComponent} from './customerLocalitiesZipCodeMap-contact-lookup-table-modal.component';
    					import {CustomerLocalitiesZipCodeMapZipCodeLookupTableModalComponent} from './customerLocalitiesZipCodeMap-zipCode-lookup-table-modal.component';
    					import {CustomerLocalitiesZipCodeMapCityLookupTableModalComponent} from './customerLocalitiesZipCodeMap-city-lookup-table-modal.component';
    					import {CustomerLocalitiesZipCodeMapStateLookupTableModalComponent} from './customerLocalitiesZipCodeMap-state-lookup-table-modal.component';
    					import {CustomerLocalitiesZipCodeMapCountryLookupTableModalComponent} from './customerLocalitiesZipCodeMap-country-lookup-table-modal.component';
    					import {CustomerLocalitiesZipCodeMapHubLookupTableModalComponent} from './customerLocalitiesZipCodeMap-hub-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        CustomerLocalitiesZipCodeMapsComponent,
        CreateOrEditCustomerLocalitiesZipCodeMapModalComponent,
        ViewCustomerLocalitiesZipCodeMapModalComponent,
        
    					CustomerLocalitiesZipCodeMapContactLookupTableModalComponent,
    					CustomerLocalitiesZipCodeMapZipCodeLookupTableModalComponent,
    					CustomerLocalitiesZipCodeMapCityLookupTableModalComponent,
    					CustomerLocalitiesZipCodeMapStateLookupTableModalComponent,
    					CustomerLocalitiesZipCodeMapCountryLookupTableModalComponent,
    					CustomerLocalitiesZipCodeMapHubLookupTableModalComponent,
    ],
    imports: [AppSharedModule, CustomerLocalitiesZipCodeMapRoutingModule , AdminSharedModule ],
    
})
export class CustomerLocalitiesZipCodeMapModule {
}
