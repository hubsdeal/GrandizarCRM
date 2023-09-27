import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {RewardPointAwardSettingRoutingModule} from './rewardPointAwardSetting-routing.module';
import {RewardPointAwardSettingsComponent} from './rewardPointAwardSettings.component';
import {CreateOrEditRewardPointAwardSettingModalComponent} from './create-or-edit-rewardPointAwardSetting-modal.component';
import {ViewRewardPointAwardSettingModalComponent} from './view-rewardPointAwardSetting-modal.component';
import {RewardPointAwardSettingRewardPointTypeLookupTableModalComponent} from './rewardPointAwardSetting-rewardPointType-lookup-table-modal.component';
    					import {RewardPointAwardSettingStoreLookupTableModalComponent} from './rewardPointAwardSetting-store-lookup-table-modal.component';
    					import {RewardPointAwardSettingProductLookupTableModalComponent} from './rewardPointAwardSetting-product-lookup-table-modal.component';
    					import {RewardPointAwardSettingMembershipTypeLookupTableModalComponent} from './rewardPointAwardSetting-membershipType-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        RewardPointAwardSettingsComponent,
        CreateOrEditRewardPointAwardSettingModalComponent,
        ViewRewardPointAwardSettingModalComponent,
        
    					RewardPointAwardSettingRewardPointTypeLookupTableModalComponent,
    					RewardPointAwardSettingStoreLookupTableModalComponent,
    					RewardPointAwardSettingProductLookupTableModalComponent,
    					RewardPointAwardSettingMembershipTypeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, RewardPointAwardSettingRoutingModule , AdminSharedModule ],
    
})
export class RewardPointAwardSettingModule {
}
