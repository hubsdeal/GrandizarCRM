import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TimesheetRoutingModule } from './timesheet-routing.module';
import { TimesheetsComponent } from './timesheets.component';
import { CreateOrEditTimesheetModalComponent } from './create-or-edit-timesheet-modal.component';
import { ViewTimesheetModalComponent } from './view-timesheet-modal.component';
import { TimesheetEmployeeLookupTableModalComponent } from './timesheet-employee-lookup-table-modal.component';
import { TimesheetStoreLookupTableModalComponent } from './timesheet-store-lookup-table-modal.component';
import { TimesheetBusinessLookupTableModalComponent } from './timesheet-business-lookup-table-modal.component';
import { TimesheetJobLookupTableModalComponent } from './timesheet-job-lookup-table-modal.component';

@NgModule({
	declarations: [
		TimesheetsComponent,
		CreateOrEditTimesheetModalComponent,
		ViewTimesheetModalComponent,

		TimesheetEmployeeLookupTableModalComponent,
		TimesheetStoreLookupTableModalComponent,
		TimesheetBusinessLookupTableModalComponent,
		TimesheetJobLookupTableModalComponent,
		TimesheetEmployeeLookupTableModalComponent,
	],
	imports: [AppSharedModule, TimesheetRoutingModule, AdminSharedModule],

})
export class TimesheetModule {
}
