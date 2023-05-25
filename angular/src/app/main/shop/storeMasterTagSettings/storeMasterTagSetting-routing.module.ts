import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreMasterTagSettingsComponent } from './storeMasterTagSettings.component';

const routes: Routes = [
    {
        path: '',
        component: StoreMasterTagSettingsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreMasterTagSettingRoutingModule {}
