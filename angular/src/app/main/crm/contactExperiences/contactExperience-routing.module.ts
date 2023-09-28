import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ContactExperiencesComponent} from './contactExperiences.component';



const routes: Routes = [
    {
        path: '',
        component: ContactExperiencesComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContactExperienceRoutingModule {
}
