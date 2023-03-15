import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DocumentTypesComponent } from './documentTypes.component';

const routes: Routes = [
    {
        path: '',
        component: DocumentTypesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DocumentTypeRoutingModule {}
