import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessDocumentsComponent } from './businessDocuments.component';

const routes: Routes = [
    {
        path: '',
        component: BusinessDocumentsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BusinessDocumentRoutingModule {}
