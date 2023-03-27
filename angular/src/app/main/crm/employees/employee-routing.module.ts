import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeDashboardComponent } from './employee-dashboard/employee-dashboard.component';
import { EmployeesComponent } from './employees.component';

const routes: Routes = [
    {
        path: '',
        component: EmployeesComponent,
        pathMatch: 'full',
    },
    {
        path: 'dashboard/:employeeId',
        component: EmployeeDashboardComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EmployeeRoutingModule {}
