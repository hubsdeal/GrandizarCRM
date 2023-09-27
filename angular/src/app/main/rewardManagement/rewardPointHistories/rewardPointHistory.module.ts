import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {RewardPointHistoryRoutingModule} from './rewardPointHistory-routing.module';
import {RewardPointHistoriesComponent} from './rewardPointHistories.component';
import {CreateOrEditRewardPointHistoryModalComponent} from './create-or-edit-rewardPointHistory-modal.component';
import {ViewRewardPointHistoryModalComponent} from './view-rewardPointHistory-modal.component';
import {RewardPointHistoryRewardPointTypeLookupTableModalComponent} from './rewardPointHistory-rewardPointType-lookup-table-modal.component';
    					import {RewardPointHistoryOrderLookupTableModalComponent} from './rewardPointHistory-order-lookup-table-modal.component';
    					import {RewardPointHistoryContactLookupTableModalComponent} from './rewardPointHistory-contact-lookup-table-modal.component';
    					import {RewardPointHistoryJobLookupTableModalComponent} from './rewardPointHistory-job-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        RewardPointHistoriesComponent,
        CreateOrEditRewardPointHistoryModalComponent,
        ViewRewardPointHistoryModalComponent,
        
    					RewardPointHistoryRewardPointTypeLookupTableModalComponent,
    					RewardPointHistoryOrderLookupTableModalComponent,
    					RewardPointHistoryContactLookupTableModalComponent,
    					RewardPointHistoryJobLookupTableModalComponent,
    ],
    imports: [AppSharedModule, RewardPointHistoryRoutingModule , AdminSharedModule ],
    
})
export class RewardPointHistoryModule {
}
