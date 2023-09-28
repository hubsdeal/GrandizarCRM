import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {JobReferralFeeSplitPoliciesComponent} from './jobReferralFeeSplitPolicies.component';



const routes: Routes = [
    {
        path: '',
        component: JobReferralFeeSplitPoliciesComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class JobReferralFeeSplitPolicyRoutingModule {
}
