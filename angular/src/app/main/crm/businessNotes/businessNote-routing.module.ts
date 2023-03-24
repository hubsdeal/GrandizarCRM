import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessNotesComponent } from './businessNotes.component';

const routes: Routes = [
    {
        path: '',
        component: BusinessNotesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BusinessNoteRoutingModule {}
