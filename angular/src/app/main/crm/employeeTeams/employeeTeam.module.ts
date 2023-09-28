import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {EmployeeTeamRoutingModule} from './employeeTeam-routing.module';
import {EmployeeTeamsComponent} from './employeeTeams.component';
import {CreateOrEditEmployeeTeamModalComponent} from './create-or-edit-employeeTeam-modal.component';
import {ViewEmployeeTeamModalComponent} from './view-employeeTeam-modal.component';
import {EmployeeTeamEmployeeLookupTableModalComponent} from './employeeTeam-employee-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        EmployeeTeamsComponent,
        CreateOrEditEmployeeTeamModalComponent,
        ViewEmployeeTeamModalComponent,
        
    					EmployeeTeamEmployeeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, EmployeeTeamRoutingModule , AdminSharedModule ],
    
})
export class EmployeeTeamModule {
}
