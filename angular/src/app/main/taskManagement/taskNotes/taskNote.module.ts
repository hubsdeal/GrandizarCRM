import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {TaskNoteRoutingModule} from './taskNote-routing.module';
import {TaskNotesComponent} from './taskNotes.component';
import {CreateOrEditTaskNoteModalComponent} from './create-or-edit-taskNote-modal.component';
import {ViewTaskNoteModalComponent} from './view-taskNote-modal.component';
import {TaskNoteTaskEventLookupTableModalComponent} from './taskNote-taskEvent-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        TaskNotesComponent,
        CreateOrEditTaskNoteModalComponent,
        ViewTaskNoteModalComponent,
        
    					TaskNoteTaskEventLookupTableModalComponent,
    ],
    imports: [AppSharedModule, TaskNoteRoutingModule , AdminSharedModule ],
    exports:[TaskNotesComponent,CreateOrEditTaskNoteModalComponent,ViewTaskNoteModalComponent]
    
})
export class TaskNoteModule {
}
