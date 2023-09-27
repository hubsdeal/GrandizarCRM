import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {RewardPointTypesComponent} from './rewardPointTypes.component';



const routes: Routes = [
    {
        path: '',
        component: RewardPointTypesComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class RewardPointTypeRoutingModule {
}
