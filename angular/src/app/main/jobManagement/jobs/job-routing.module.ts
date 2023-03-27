import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JobDashboardComponent } from './job-dashboard/job-dashboard.component';
import { JobsComponent } from './jobs.component';

const routes: Routes = [
    {
        path: '',
        component: JobsComponent,
        pathMatch: 'full',
    },
    {
        path: 'dashboard/:jobId',
        component: JobDashboardComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class JobRoutingModule {}
