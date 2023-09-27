import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {ConnectMessageRoutingModule} from './connectMessage-routing.module';
import {ConnectMessagesComponent} from './connectMessages.component';
import {CreateOrEditConnectMessageModalComponent} from './create-or-edit-connectMessage-modal.component';
import {ViewConnectMessageModalComponent} from './view-connectMessage-modal.component';
import {ConnectMessageConnectChannelLookupTableModalComponent} from './connectMessage-connectChannel-lookup-table-modal.component';
    					import {ConnectMessageUserLookupTableModalComponent} from './connectMessage-user-lookup-table-modal.component';
    					import {ConnectMessageContactLookupTableModalComponent} from './connectMessage-contact-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        ConnectMessagesComponent,
        CreateOrEditConnectMessageModalComponent,
        ViewConnectMessageModalComponent,
        
    					ConnectMessageConnectChannelLookupTableModalComponent,
    					ConnectMessageUserLookupTableModalComponent,
    					ConnectMessageContactLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ConnectMessageRoutingModule , AdminSharedModule ],
    
})
export class ConnectMessageModule {
}
