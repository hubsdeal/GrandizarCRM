import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {TimesheetPolicyRoutingModule} from './timesheetPolicy-routing.module';
import {TimesheetPoliciesComponent} from './timesheetPolicies.component';
import {CreateOrEditTimesheetPolicyModalComponent} from './create-or-edit-timesheetPolicy-modal.component';
import {ViewTimesheetPolicyModalComponent} from './view-timesheetPolicy-modal.component';



@NgModule({
    declarations: [
        TimesheetPoliciesComponent,
        CreateOrEditTimesheetPolicyModalComponent,
        ViewTimesheetPolicyModalComponent,
        
    ],
    imports: [AppSharedModule, TimesheetPolicyRoutingModule , AdminSharedModule ],
    
})
export class TimesheetPolicyModule {
}
