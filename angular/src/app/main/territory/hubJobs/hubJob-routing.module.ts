import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HubJobsComponent} from './hubJobs.component';



const routes: Routes = [
    {
        path: '',
        component: HubJobsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubJobRoutingModule {
}
