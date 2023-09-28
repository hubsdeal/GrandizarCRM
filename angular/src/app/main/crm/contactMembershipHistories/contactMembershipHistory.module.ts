import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {ContactMembershipHistoryRoutingModule} from './contactMembershipHistory-routing.module';
import {ContactMembershipHistoriesComponent} from './contactMembershipHistories.component';
import {CreateOrEditContactMembershipHistoryModalComponent} from './create-or-edit-contactMembershipHistory-modal.component';
import {ViewContactMembershipHistoryModalComponent} from './view-contactMembershipHistory-modal.component';
import {ContactMembershipHistoryContactLookupTableModalComponent} from './contactMembershipHistory-contact-lookup-table-modal.component';
    					import {ContactMembershipHistoryMembershipTypeLookupTableModalComponent} from './contactMembershipHistory-membershipType-lookup-table-modal.component';
    					import {ContactMembershipHistoryProductLookupTableModalComponent} from './contactMembershipHistory-product-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        ContactMembershipHistoriesComponent,
        CreateOrEditContactMembershipHistoryModalComponent,
        ViewContactMembershipHistoryModalComponent,
        
    					ContactMembershipHistoryContactLookupTableModalComponent,
    					ContactMembershipHistoryMembershipTypeLookupTableModalComponent,
    					ContactMembershipHistoryProductLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ContactMembershipHistoryRoutingModule , AdminSharedModule ],
    
})
export class ContactMembershipHistoryModule {
}
