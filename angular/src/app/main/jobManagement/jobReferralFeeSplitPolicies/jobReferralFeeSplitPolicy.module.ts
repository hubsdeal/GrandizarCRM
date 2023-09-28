import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {JobReferralFeeSplitPolicyRoutingModule} from './jobReferralFeeSplitPolicy-routing.module';
import {JobReferralFeeSplitPoliciesComponent} from './jobReferralFeeSplitPolicies.component';
import {CreateOrEditJobReferralFeeSplitPolicyModalComponent} from './create-or-edit-jobReferralFeeSplitPolicy-modal.component';
import {ViewJobReferralFeeSplitPolicyModalComponent} from './view-jobReferralFeeSplitPolicy-modal.component';



@NgModule({
    declarations: [
        JobReferralFeeSplitPoliciesComponent,
        CreateOrEditJobReferralFeeSplitPolicyModalComponent,
        ViewJobReferralFeeSplitPolicyModalComponent,
        
    ],
    imports: [AppSharedModule, JobReferralFeeSplitPolicyRoutingModule , AdminSharedModule ],
    
})
export class JobReferralFeeSplitPolicyModule {
}
