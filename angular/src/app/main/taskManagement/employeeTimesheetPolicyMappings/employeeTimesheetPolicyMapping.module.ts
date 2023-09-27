import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {EmployeeTimesheetPolicyMappingRoutingModule} from './employeeTimesheetPolicyMapping-routing.module';
import {EmployeeTimesheetPolicyMappingsComponent} from './employeeTimesheetPolicyMappings.component';
import {CreateOrEditEmployeeTimesheetPolicyMappingModalComponent} from './create-or-edit-employeeTimesheetPolicyMapping-modal.component';
import {ViewEmployeeTimesheetPolicyMappingModalComponent} from './view-employeeTimesheetPolicyMapping-modal.component';
import {EmployeeTimesheetPolicyMappingEmployeeLookupTableModalComponent} from './employeeTimesheetPolicyMapping-employee-lookup-table-modal.component';
    					import {EmployeeTimesheetPolicyMappingTimesheetPolicyLookupTableModalComponent} from './employeeTimesheetPolicyMapping-timesheetPolicy-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        EmployeeTimesheetPolicyMappingsComponent,
        CreateOrEditEmployeeTimesheetPolicyMappingModalComponent,
        ViewEmployeeTimesheetPolicyMappingModalComponent,
        
    					EmployeeTimesheetPolicyMappingEmployeeLookupTableModalComponent,
    					EmployeeTimesheetPolicyMappingTimesheetPolicyLookupTableModalComponent,
    ],
    imports: [AppSharedModule, EmployeeTimesheetPolicyMappingRoutingModule , AdminSharedModule ],
    
})
export class EmployeeTimesheetPolicyMappingModule {
}
