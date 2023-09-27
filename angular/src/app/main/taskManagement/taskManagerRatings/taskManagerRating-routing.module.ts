import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {TaskManagerRatingsComponent} from './taskManagerRatings.component';



const routes: Routes = [
    {
        path: '',
        component: TaskManagerRatingsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TaskManagerRatingRoutingModule {
}
