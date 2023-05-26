import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JobTaskMapsComponent } from './jobTaskMaps.component';

const routes: Routes = [
    {
        path: '',
        component: JobTaskMapsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class JobTaskMapRoutingModule {}
