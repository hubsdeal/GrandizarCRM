import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {RewardPointPriceSettingRoutingModule} from './rewardPointPriceSetting-routing.module';
import {RewardPointPriceSettingsComponent} from './rewardPointPriceSettings.component';
import {CreateOrEditRewardPointPriceSettingModalComponent} from './create-or-edit-rewardPointPriceSetting-modal.component';
import {ViewRewardPointPriceSettingModalComponent} from './view-rewardPointPriceSetting-modal.component';
import {RewardPointPriceSettingCurrencyLookupTableModalComponent} from './rewardPointPriceSetting-currency-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        RewardPointPriceSettingsComponent,
        CreateOrEditRewardPointPriceSettingModalComponent,
        ViewRewardPointPriceSettingModalComponent,
        
    					RewardPointPriceSettingCurrencyLookupTableModalComponent,
    ],
    imports: [AppSharedModule, RewardPointPriceSettingRoutingModule , AdminSharedModule ],
    
})
export class RewardPointPriceSettingModule {
}
