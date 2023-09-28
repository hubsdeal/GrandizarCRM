import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {TimesheetTaskMapRoutingModule} from './timesheetTaskMap-routing.module';
import {TimesheetTaskMapsComponent} from './timesheetTaskMaps.component';
import {CreateOrEditTimesheetTaskMapModalComponent} from './create-or-edit-timesheetTaskMap-modal.component';
import {ViewTimesheetTaskMapModalComponent} from './view-timesheetTaskMap-modal.component';
import {TimesheetTaskMapTimesheetLookupTableModalComponent} from './timesheetTaskMap-timesheet-lookup-table-modal.component';
    					import {TimesheetTaskMapTaskEventLookupTableModalComponent} from './timesheetTaskMap-taskEvent-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        TimesheetTaskMapsComponent,
        CreateOrEditTimesheetTaskMapModalComponent,
        ViewTimesheetTaskMapModalComponent,
        
    					TimesheetTaskMapTimesheetLookupTableModalComponent,
    					TimesheetTaskMapTaskEventLookupTableModalComponent,
    ],
    imports: [AppSharedModule, TimesheetTaskMapRoutingModule , AdminSharedModule ],
    
})
export class TimesheetTaskMapModule {
}
