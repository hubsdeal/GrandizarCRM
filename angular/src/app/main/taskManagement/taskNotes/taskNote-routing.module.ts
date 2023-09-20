import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {TaskNotesComponent} from './taskNotes.component';



const routes: Routes = [
    {
        path: '',
        component: TaskNotesComponent,
        pathMatch: 'full'
    },
    
    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TaskNoteRoutingModule {
}
