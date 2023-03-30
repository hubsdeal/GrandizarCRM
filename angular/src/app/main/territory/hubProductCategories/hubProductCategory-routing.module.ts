import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HubProductCategoriesComponent } from './hubProductCategories.component';

const routes: Routes = [
    {
        path: '',
        component: HubProductCategoriesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubProductCategoryRoutingModule {}
