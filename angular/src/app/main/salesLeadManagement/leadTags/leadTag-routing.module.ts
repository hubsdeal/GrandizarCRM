import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LeadTagsComponent } from './leadTags.component';

const routes: Routes = [
    {
        path: '',
        component: LeadTagsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LeadTagRoutingModule {}
