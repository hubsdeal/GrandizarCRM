import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ContractFinancialTermsComponent} from './contractFinancialTerms.component';



const routes: Routes = [
    {
        path: '',
        component: ContractFinancialTermsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContractFinancialTermRoutingModule {
}
