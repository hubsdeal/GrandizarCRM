import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {TimesheetTaskMapsComponent} from './timesheetTaskMaps.component';



const routes: Routes = [
    {
        path: '',
        component: TimesheetTaskMapsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TimesheetTaskMapRoutingModule {
}
