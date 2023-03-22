import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContactDashboardComponent } from './contact-dashboard/contact-dashboard.component';
import { ContactsComponent } from './contacts.component';

const routes: Routes = [
    {
        path: '',
        component: ContactsComponent,
        pathMatch: 'full',
    },
    {
        path: 'dashboard/:contactId',
        component: ContactDashboardComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContactRoutingModule {}
