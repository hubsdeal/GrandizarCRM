import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MarketplaceCommissionTypesComponent } from './marketplaceCommissionTypes.component';

const routes: Routes = [
    {
        path: '',
        component: MarketplaceCommissionTypesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MarketplaceCommissionTypeRoutingModule {}
