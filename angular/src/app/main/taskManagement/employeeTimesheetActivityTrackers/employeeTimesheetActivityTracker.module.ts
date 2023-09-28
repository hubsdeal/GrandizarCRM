import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {EmployeeTimesheetActivityTrackerRoutingModule} from './employeeTimesheetActivityTracker-routing.module';
import {EmployeeTimesheetActivityTrackersComponent} from './employeeTimesheetActivityTrackers.component';
import {CreateOrEditEmployeeTimesheetActivityTrackerModalComponent} from './create-or-edit-employeeTimesheetActivityTracker-modal.component';
import {ViewEmployeeTimesheetActivityTrackerModalComponent} from './view-employeeTimesheetActivityTracker-modal.component';
import {EmployeeTimesheetActivityTrackerTimesheetLookupTableModalComponent} from './employeeTimesheetActivityTracker-timesheet-lookup-table-modal.component';
    					import {EmployeeTimesheetActivityTrackerTaskEventLookupTableModalComponent} from './employeeTimesheetActivityTracker-taskEvent-lookup-table-modal.component';
    					import {EmployeeTimesheetActivityTrackerEmployeeLookupTableModalComponent} from './employeeTimesheetActivityTracker-employee-lookup-table-modal.component';
    					import {EmployeeTimesheetActivityTrackerTimesheetPolicyLookupTableModalComponent} from './employeeTimesheetActivityTracker-timesheetPolicy-lookup-table-modal.component';
    					import {EmployeeTimesheetActivityTrackerTaskWorkItemLookupTableModalComponent} from './employeeTimesheetActivityTracker-taskWorkItem-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        EmployeeTimesheetActivityTrackersComponent,
        CreateOrEditEmployeeTimesheetActivityTrackerModalComponent,
        ViewEmployeeTimesheetActivityTrackerModalComponent,
        
    					EmployeeTimesheetActivityTrackerTimesheetLookupTableModalComponent,
    					EmployeeTimesheetActivityTrackerTaskEventLookupTableModalComponent,
    					EmployeeTimesheetActivityTrackerEmployeeLookupTableModalComponent,
    					EmployeeTimesheetActivityTrackerTimesheetPolicyLookupTableModalComponent,
    					EmployeeTimesheetActivityTrackerTaskWorkItemLookupTableModalComponent,
    ],
    imports: [AppSharedModule, EmployeeTimesheetActivityTrackerRoutingModule , AdminSharedModule ],
    
})
export class EmployeeTimesheetActivityTrackerModule {
}
