import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreNotesComponent } from './storeNotes.component';

const routes: Routes = [
    {
        path: '',
        component: StoreNotesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreNoteRoutingModule {}
