import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { OrderTeamRoutingModule } from './orderTeam-routing.module';
import { OrderTeamsComponent } from './orderTeams.component';
import { CreateOrEditOrderTeamModalComponent } from './create-or-edit-orderTeam-modal.component';
import { ViewOrderTeamModalComponent } from './view-orderTeam-modal.component';
import { OrderTeamOrderLookupTableModalComponent } from './orderTeam-order-lookup-table-modal.component';
import { OrderTeamEmployeeLookupTableModalComponent } from './orderTeam-employee-lookup-table-modal.component';

@NgModule({
    declarations: [
        OrderTeamsComponent,
        CreateOrEditOrderTeamModalComponent,
        ViewOrderTeamModalComponent,

        OrderTeamOrderLookupTableModalComponent,
        OrderTeamEmployeeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, OrderTeamRoutingModule, AdminSharedModule],
})
export class OrderTeamModule {}
