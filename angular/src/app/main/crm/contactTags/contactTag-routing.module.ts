import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContactTagsComponent } from './contactTags.component';

const routes: Routes = [
    {
        path: '',
        component: ContactTagsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContactTagRoutingModule {}
