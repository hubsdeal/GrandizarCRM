import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductNotesComponent } from './productNotes.component';

const routes: Routes = [
    {
        path: '',
        component: ProductNotesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductNoteRoutingModule {}
