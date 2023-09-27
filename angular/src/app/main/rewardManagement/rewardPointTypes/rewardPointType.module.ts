import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {RewardPointTypeRoutingModule} from './rewardPointType-routing.module';
import {RewardPointTypesComponent} from './rewardPointTypes.component';
import {CreateOrEditRewardPointTypeModalComponent} from './create-or-edit-rewardPointType-modal.component';
import {ViewRewardPointTypeModalComponent} from './view-rewardPointType-modal.component';



@NgModule({
    declarations: [
        RewardPointTypesComponent,
        CreateOrEditRewardPointTypeModalComponent,
        ViewRewardPointTypeModalComponent,
        
    ],
    imports: [AppSharedModule, RewardPointTypeRoutingModule , AdminSharedModule ],
    
})
export class RewardPointTypeModule {
}
