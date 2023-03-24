import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JobStatusTypesComponent } from './jobStatusTypes.component';

const routes: Routes = [
    {
        path: '',
        component: JobStatusTypesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class JobStatusTypeRoutingModule {}
