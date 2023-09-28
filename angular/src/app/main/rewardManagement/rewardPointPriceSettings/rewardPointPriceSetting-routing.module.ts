import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {RewardPointPriceSettingsComponent} from './rewardPointPriceSettings.component';



const routes: Routes = [
    {
        path: '',
        component: RewardPointPriceSettingsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class RewardPointPriceSettingRoutingModule {
}
