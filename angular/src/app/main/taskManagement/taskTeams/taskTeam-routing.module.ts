import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaskTeamsComponent } from './taskTeams.component';

const routes: Routes = [
    {
        path: '',
        component: TaskTeamsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TaskTeamRoutingModule {}
