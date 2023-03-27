import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { LeadReferralRewardRoutingModule } from './leadReferralReward-routing.module';
import { LeadReferralRewardsComponent } from './leadReferralRewards.component';
import { CreateOrEditLeadReferralRewardModalComponent } from './create-or-edit-leadReferralReward-modal.component';
import { ViewLeadReferralRewardModalComponent } from './view-leadReferralReward-modal.component';
import { LeadReferralRewardLeadLookupTableModalComponent } from './leadReferralReward-lead-lookup-table-modal.component';
import { LeadReferralRewardContactLookupTableModalComponent } from './leadReferralReward-contact-lookup-table-modal.component';

@NgModule({
    declarations: [
        LeadReferralRewardsComponent,
        CreateOrEditLeadReferralRewardModalComponent,
        ViewLeadReferralRewardModalComponent,

        LeadReferralRewardLeadLookupTableModalComponent,
        LeadReferralRewardContactLookupTableModalComponent,
    ],
    imports: [AppSharedModule, LeadReferralRewardRoutingModule, AdminSharedModule],
})
export class LeadReferralRewardModule {}
