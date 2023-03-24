import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductTagsComponent } from './productTags.component';

const routes: Routes = [
    {
        path: '',
        component: ProductTagsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductTagRoutingModule {}
