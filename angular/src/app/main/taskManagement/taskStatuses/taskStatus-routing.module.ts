import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaskStatusesComponent } from './taskStatuses.component';

const routes: Routes = [
    {
        path: '',
        component: TaskStatusesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TaskStatusRoutingModule {}
