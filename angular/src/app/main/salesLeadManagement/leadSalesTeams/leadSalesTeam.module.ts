import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { LeadSalesTeamRoutingModule } from './leadSalesTeam-routing.module';
import { LeadSalesTeamsComponent } from './leadSalesTeams.component';
import { CreateOrEditLeadSalesTeamModalComponent } from './create-or-edit-leadSalesTeam-modal.component';
import { ViewLeadSalesTeamModalComponent } from './view-leadSalesTeam-modal.component';
import { LeadSalesTeamLeadLookupTableModalComponent } from './leadSalesTeam-lead-lookup-table-modal.component';
import { LeadSalesTeamEmployeeLookupTableModalComponent } from './leadSalesTeam-employee-lookup-table-modal.component';

@NgModule({
    declarations: [
        LeadSalesTeamsComponent,
        CreateOrEditLeadSalesTeamModalComponent,
        ViewLeadSalesTeamModalComponent,

        LeadSalesTeamLeadLookupTableModalComponent,
        LeadSalesTeamEmployeeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, LeadSalesTeamRoutingModule, AdminSharedModule],
})
export class LeadSalesTeamModule {}
