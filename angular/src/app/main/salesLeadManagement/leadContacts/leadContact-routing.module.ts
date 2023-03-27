import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LeadContactsComponent } from './leadContacts.component';

const routes: Routes = [
    {
        path: '',
        component: LeadContactsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LeadContactRoutingModule {}
