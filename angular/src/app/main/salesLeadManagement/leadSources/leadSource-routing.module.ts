import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LeadSourcesComponent } from './leadSources.component';

const routes: Routes = [
    {
        path: '',
        component: LeadSourcesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LeadSourceRoutingModule {}
