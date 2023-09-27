import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {ContactReferralContactRoutingModule} from './contactReferralContact-routing.module';
import {ContactReferralContactsComponent} from './contactReferralContacts.component';
import {CreateOrEditContactReferralContactModalComponent} from './create-or-edit-contactReferralContact-modal.component';
import {ViewContactReferralContactModalComponent} from './view-contactReferralContact-modal.component';
import {ContactReferralContactContactLookupTableModalComponent} from './contactReferralContact-contact-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        ContactReferralContactsComponent,
        CreateOrEditContactReferralContactModalComponent,
        ViewContactReferralContactModalComponent,
        
    					ContactReferralContactContactLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ContactReferralContactRoutingModule , AdminSharedModule ],
    
})
export class ContactReferralContactModule {
}
