import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LeadPipelineStatusesComponent } from './leadPipelineStatuses.component';

const routes: Routes = [
    {
        path: '',
        component: LeadPipelineStatusesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LeadPipelineStatusRoutingModule {}
