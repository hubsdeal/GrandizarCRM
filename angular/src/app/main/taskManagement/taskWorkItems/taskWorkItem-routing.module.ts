import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaskWorkItemsComponent } from './taskWorkItems.component';

const routes: Routes = [
    {
        path: '',
        component: TaskWorkItemsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TaskWorkItemRoutingModule {}
