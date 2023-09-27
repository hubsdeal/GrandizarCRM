import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ContactOrApplicantTeamsComponent} from './contactOrApplicantTeams.component';



const routes: Routes = [
    {
        path: '',
        component: ContactOrApplicantTeamsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContactOrApplicantTeamRoutingModule {
}
