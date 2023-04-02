import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { OrderfulfillmentTeamRoutingModule } from './orderfulfillmentTeam-routing.module';
import { OrderfulfillmentTeamsComponent } from './orderfulfillmentTeams.component';
import { CreateOrEditOrderfulfillmentTeamModalComponent } from './create-or-edit-orderfulfillmentTeam-modal.component';
import { ViewOrderfulfillmentTeamModalComponent } from './view-orderfulfillmentTeam-modal.component';
import { OrderfulfillmentTeamOrderLookupTableModalComponent } from './orderfulfillmentTeam-order-lookup-table-modal.component';
import { OrderfulfillmentTeamEmployeeLookupTableModalComponent } from './orderfulfillmentTeam-employee-lookup-table-modal.component';
import { OrderfulfillmentTeamContactLookupTableModalComponent } from './orderfulfillmentTeam-contact-lookup-table-modal.component';
import { OrderfulfillmentTeamUserLookupTableModalComponent } from './orderfulfillmentTeam-user-lookup-table-modal.component';

@NgModule({
    declarations: [
        OrderfulfillmentTeamsComponent,
        CreateOrEditOrderfulfillmentTeamModalComponent,
        ViewOrderfulfillmentTeamModalComponent,

        OrderfulfillmentTeamOrderLookupTableModalComponent,
        OrderfulfillmentTeamEmployeeLookupTableModalComponent,
        OrderfulfillmentTeamContactLookupTableModalComponent,
        OrderfulfillmentTeamUserLookupTableModalComponent,
    ],
    imports: [AppSharedModule, OrderfulfillmentTeamRoutingModule, AdminSharedModule],
})
export class OrderfulfillmentTeamModule {}
