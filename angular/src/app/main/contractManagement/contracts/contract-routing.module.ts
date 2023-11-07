import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ContractsComponent} from './contracts.component';
import { ContractDashboardComponent } from './contract-dashboard/contract-dashboard.component';



const routes: Routes = [
    {
        path: '',
        component: ContractsComponent,
        pathMatch: 'full'
    },
    {
        path: 'dashboard/:contractId',
        component: ContractDashboardComponent,
    },
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContractRoutingModule {
}
