import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubAccountTeamRoutingModule } from './hubAccountTeam-routing.module';
import { HubAccountTeamsComponent } from './hubAccountTeams.component';
import { CreateOrEditHubAccountTeamModalComponent } from './create-or-edit-hubAccountTeam-modal.component';
import { ViewHubAccountTeamModalComponent } from './view-hubAccountTeam-modal.component';
import { HubAccountTeamHubLookupTableModalComponent } from './hubAccountTeam-hub-lookup-table-modal.component';
import { HubAccountTeamEmployeeLookupTableModalComponent } from './hubAccountTeam-employee-lookup-table-modal.component';
import { HubAccountTeamUserLookupTableModalComponent } from './hubAccountTeam-user-lookup-table-modal.component';

@NgModule({
    declarations: [
        HubAccountTeamsComponent,
        CreateOrEditHubAccountTeamModalComponent,
        ViewHubAccountTeamModalComponent,

        HubAccountTeamHubLookupTableModalComponent,
        HubAccountTeamEmployeeLookupTableModalComponent,
        HubAccountTeamUserLookupTableModalComponent,
    ],
    imports: [AppSharedModule, HubAccountTeamRoutingModule, AdminSharedModule],
})
export class HubAccountTeamModule {}
