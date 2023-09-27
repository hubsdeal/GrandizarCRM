import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {JobReferralFeeDetailsComponent} from './jobReferralFeeDetails.component';



const routes: Routes = [
    {
        path: '',
        component: JobReferralFeeDetailsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class JobReferralFeeDetailRoutingModule {
}
