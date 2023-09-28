import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {EmployeeTimesheetActivityTrackersComponent} from './employeeTimesheetActivityTrackers.component';



const routes: Routes = [
    {
        path: '',
        component: EmployeeTimesheetActivityTrackersComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EmployeeTimesheetActivityTrackerRoutingModule {
}
