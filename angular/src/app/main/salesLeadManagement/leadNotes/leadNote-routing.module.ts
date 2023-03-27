import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LeadNotesComponent } from './leadNotes.component';

const routes: Routes = [
    {
        path: '',
        component: LeadNotesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LeadNoteRoutingModule {}
