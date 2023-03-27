import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LeadReferralRewardsComponent } from './leadReferralRewards.component';

const routes: Routes = [
    {
        path: '',
        component: LeadReferralRewardsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LeadReferralRewardRoutingModule {}
