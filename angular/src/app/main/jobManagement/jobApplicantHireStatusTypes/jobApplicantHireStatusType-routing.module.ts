import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {JobApplicantHireStatusTypesComponent} from './jobApplicantHireStatusTypes.component';



const routes: Routes = [
    {
        path: '',
        component: JobApplicantHireStatusTypesComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class JobApplicantHireStatusTypeRoutingModule {
}
