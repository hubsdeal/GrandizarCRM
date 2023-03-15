import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MasterTagCategoriesComponent } from './masterTagCategories.component';

const routes: Routes = [
    {
        path: '',
        component: MasterTagCategoriesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MasterTagCategoryRoutingModule {}
