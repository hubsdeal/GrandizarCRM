import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreMarketplaceCommissionSettingsComponent } from './storeMarketplaceCommissionSettings.component';

const routes: Routes = [
    {
        path: '',
        component: StoreMarketplaceCommissionSettingsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreMarketplaceCommissionSettingRoutingModule {}
