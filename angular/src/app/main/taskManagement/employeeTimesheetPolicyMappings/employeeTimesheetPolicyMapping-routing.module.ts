import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {EmployeeTimesheetPolicyMappingsComponent} from './employeeTimesheetPolicyMappings.component';



const routes: Routes = [
    {
        path: '',
        component: EmployeeTimesheetPolicyMappingsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EmployeeTimesheetPolicyMappingRoutingModule {
}
