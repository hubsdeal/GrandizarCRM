import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreTagSettingCategoriesComponent } from './storeTagSettingCategories.component';

const routes: Routes = [
    {
        path: '',
        component: StoreTagSettingCategoriesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreTagSettingCategoryRoutingModule {}
