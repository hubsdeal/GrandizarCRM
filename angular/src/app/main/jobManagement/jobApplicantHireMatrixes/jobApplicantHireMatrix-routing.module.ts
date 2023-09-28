import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {JobApplicantHireMatrixesComponent} from './jobApplicantHireMatrixes.component';



const routes: Routes = [
    {
        path: '',
        component: JobApplicantHireMatrixesComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class JobApplicantHireMatrixRoutingModule {
}
