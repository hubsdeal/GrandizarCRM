import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaskTagsComponent } from './taskTags.component';

const routes: Routes = [
    {
        path: '',
        component: TaskTagsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TaskTagRoutingModule {}
