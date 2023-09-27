import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {TimesheetsComponent} from './timesheets.component';



const routes: Routes = [
    {
        path: '',
        component: TimesheetsComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TimesheetRoutingModule {
}
