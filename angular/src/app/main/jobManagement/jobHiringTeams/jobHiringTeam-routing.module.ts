import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {JobHiringTeamsComponent} from './jobHiringTeams.component';



const routes: Routes = [
    {
        path: '',
        component: JobHiringTeamsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class JobHiringTeamRoutingModule {
}
