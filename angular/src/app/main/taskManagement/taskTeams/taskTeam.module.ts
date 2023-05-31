import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TaskTeamRoutingModule } from './taskTeam-routing.module';
import { TaskTeamsComponent } from './taskTeams.component';
import { CreateOrEditTaskTeamModalComponent } from './create-or-edit-taskTeam-modal.component';
import { ViewTaskTeamModalComponent } from './view-taskTeam-modal.component';
import { TaskTeamTaskEventLookupTableModalComponent } from './taskTeam-taskEvent-lookup-table-modal.component';
import { TaskTeamEmployeeLookupTableModalComponent } from './taskTeam-employee-lookup-table-modal.component';
import { TaskTeamContactLookupTableModalComponent } from './taskTeam-contact-lookup-table-modal.component';

@NgModule({
    declarations: [
        TaskTeamsComponent,
        CreateOrEditTaskTeamModalComponent,
        ViewTaskTeamModalComponent,

        TaskTeamTaskEventLookupTableModalComponent,
        TaskTeamEmployeeLookupTableModalComponent,
        TaskTeamContactLookupTableModalComponent,
    ],
    imports: [AppSharedModule, TaskTeamRoutingModule, AdminSharedModule],
})
export class TaskTeamModule {}
