import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {RewardPointAwardSettingsComponent} from './rewardPointAwardSettings.component';



const routes: Routes = [
    {
        path: '',
        component: RewardPointAwardSettingsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class RewardPointAwardSettingRoutingModule {
}
