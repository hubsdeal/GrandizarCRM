import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductMasterTagSettingsComponent } from './productMasterTagSettings.component';

const routes: Routes = [
    {
        path: '',
        component: ProductMasterTagSettingsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductMasterTagSettingRoutingModule {}
