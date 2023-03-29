import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SubscriptionTypesComponent } from './subscriptionTypes.component';

const routes: Routes = [
    {
        path: '',
        component: SubscriptionTypesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class SubscriptionTypeRoutingModule {}
