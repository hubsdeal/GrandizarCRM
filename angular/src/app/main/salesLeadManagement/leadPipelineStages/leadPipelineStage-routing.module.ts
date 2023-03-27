import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LeadPipelineStagesComponent } from './leadPipelineStages.component';

const routes: Routes = [
    {
        path: '',
        component: LeadPipelineStagesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LeadPipelineStageRoutingModule {}
