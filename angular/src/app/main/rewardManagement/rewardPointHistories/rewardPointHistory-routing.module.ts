import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {RewardPointHistoriesComponent} from './rewardPointHistories.component';



const routes: Routes = [
    {
        path: '',
        component: RewardPointHistoriesComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class RewardPointHistoryRoutingModule {
}
