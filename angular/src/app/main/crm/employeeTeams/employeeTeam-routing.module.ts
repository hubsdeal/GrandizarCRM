import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {EmployeeTeamsComponent} from './employeeTeams.component';



const routes: Routes = [
    {
        path: '',
        component: EmployeeTeamsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EmployeeTeamRoutingModule {
}
