import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HubContactsComponent } from './hubContacts.component';

const routes: Routes = [
    {
        path: '',
        component: HubContactsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubContactRoutingModule {}
