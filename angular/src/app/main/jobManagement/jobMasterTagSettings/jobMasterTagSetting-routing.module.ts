import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JobMasterTagSettingsComponent } from './jobMasterTagSettings.component';

const routes: Routes = [
    {
        path: '',
        component: JobMasterTagSettingsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class JobMasterTagSettingRoutingModule {}
