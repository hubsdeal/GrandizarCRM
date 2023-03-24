import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BusinessAccountTeamRoutingModule } from './businessAccountTeam-routing.module';
import { BusinessAccountTeamsComponent } from './businessAccountTeams.component';
import { CreateOrEditBusinessAccountTeamModalComponent } from './create-or-edit-businessAccountTeam-modal.component';
import { ViewBusinessAccountTeamModalComponent } from './view-businessAccountTeam-modal.component';
import { BusinessAccountTeamBusinessLookupTableModalComponent } from './businessAccountTeam-business-lookup-table-modal.component';
import { BusinessAccountTeamEmployeeLookupTableModalComponent } from './businessAccountTeam-employee-lookup-table-modal.component';

@NgModule({
    declarations: [
        BusinessAccountTeamsComponent,
        CreateOrEditBusinessAccountTeamModalComponent,
        ViewBusinessAccountTeamModalComponent,

        BusinessAccountTeamBusinessLookupTableModalComponent,
        BusinessAccountTeamEmployeeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, BusinessAccountTeamRoutingModule, AdminSharedModule],
})
export class BusinessAccountTeamModule {}
