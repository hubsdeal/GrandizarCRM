import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessTagsComponent } from './businessTags.component';

const routes: Routes = [
    {
        path: '',
        component: BusinessTagsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BusinessTagRoutingModule {}
