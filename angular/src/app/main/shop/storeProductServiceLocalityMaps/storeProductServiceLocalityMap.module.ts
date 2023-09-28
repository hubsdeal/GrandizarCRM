import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {StoreProductServiceLocalityMapRoutingModule} from './storeProductServiceLocalityMap-routing.module';
import {StoreProductServiceLocalityMapsComponent} from './storeProductServiceLocalityMaps.component';
import {CreateOrEditStoreProductServiceLocalityMapModalComponent} from './create-or-edit-storeProductServiceLocalityMap-modal.component';
import {ViewStoreProductServiceLocalityMapModalComponent} from './view-storeProductServiceLocalityMap-modal.component';
import {StoreProductServiceLocalityMapProductLookupTableModalComponent} from './storeProductServiceLocalityMap-product-lookup-table-modal.component';
    					import {StoreProductServiceLocalityMapStoreLookupTableModalComponent} from './storeProductServiceLocalityMap-store-lookup-table-modal.component';
    					import {StoreProductServiceLocalityMapZipCodeLookupTableModalComponent} from './storeProductServiceLocalityMap-zipCode-lookup-table-modal.component';
    					import {StoreProductServiceLocalityMapCityLookupTableModalComponent} from './storeProductServiceLocalityMap-city-lookup-table-modal.component';
    					import {StoreProductServiceLocalityMapStateLookupTableModalComponent} from './storeProductServiceLocalityMap-state-lookup-table-modal.component';
    					import {StoreProductServiceLocalityMapCountryLookupTableModalComponent} from './storeProductServiceLocalityMap-country-lookup-table-modal.component';
    					import {StoreProductServiceLocalityMapHubLookupTableModalComponent} from './storeProductServiceLocalityMap-hub-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        StoreProductServiceLocalityMapsComponent,
        CreateOrEditStoreProductServiceLocalityMapModalComponent,
        ViewStoreProductServiceLocalityMapModalComponent,
        
    					StoreProductServiceLocalityMapProductLookupTableModalComponent,
    					StoreProductServiceLocalityMapStoreLookupTableModalComponent,
    					StoreProductServiceLocalityMapZipCodeLookupTableModalComponent,
    					StoreProductServiceLocalityMapCityLookupTableModalComponent,
    					StoreProductServiceLocalityMapStateLookupTableModalComponent,
    					StoreProductServiceLocalityMapCountryLookupTableModalComponent,
    					StoreProductServiceLocalityMapHubLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreProductServiceLocalityMapRoutingModule , AdminSharedModule ],
    
})
export class StoreProductServiceLocalityMapModule {
}
