import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {ContactAddressRoutingModule} from './contactAddress-routing.module';
import {ContactAddressesComponent} from './contactAddresses.component';
import {CreateOrEditContactAddressModalComponent} from './create-or-edit-contactAddress-modal.component';
import {ViewContactAddressModalComponent} from './view-contactAddress-modal.component';
import {ContactAddressContactLookupTableModalComponent} from './contactAddress-contact-lookup-table-modal.component';
    					import {ContactAddressCountryLookupTableModalComponent} from './contactAddress-country-lookup-table-modal.component';
    					import {ContactAddressStateLookupTableModalComponent} from './contactAddress-state-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        ContactAddressesComponent,
        CreateOrEditContactAddressModalComponent,
        ViewContactAddressModalComponent,
        
    					ContactAddressContactLookupTableModalComponent,
    					ContactAddressCountryLookupTableModalComponent,
    					ContactAddressStateLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ContactAddressRoutingModule , AdminSharedModule ],
    
})
export class ContactAddressModule {
}
