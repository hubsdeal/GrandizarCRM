import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreTagsComponent } from './storeTags.component';

const routes: Routes = [
    {
        path: '',
        component: StoreTagsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreTagRoutingModule {}
