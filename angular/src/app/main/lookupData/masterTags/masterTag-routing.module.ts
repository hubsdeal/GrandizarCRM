import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MasterTagsComponent } from './masterTags.component';

const routes: Routes = [
    {
        path: '',
        component: MasterTagsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MasterTagRoutingModule {}
