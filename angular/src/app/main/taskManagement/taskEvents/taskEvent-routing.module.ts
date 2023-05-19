import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaskEventsComponent } from './taskEvents.component';
import { TaskEventsDashboardComponent } from './task-events-dashboard/task-events-dashboard.component';

const routes: Routes = [
    {
        path: '',
        component: TaskEventsComponent,
        pathMatch: 'full',
    },
    {
        path: 'dashboard/:taskEventId',
        component: TaskEventsDashboardComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TaskEventRoutingModule {}
