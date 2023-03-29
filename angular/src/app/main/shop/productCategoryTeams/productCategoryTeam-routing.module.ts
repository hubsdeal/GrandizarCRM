import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductCategoryTeamsComponent } from './productCategoryTeams.component';

const routes: Routes = [
    {
        path: '',
        component: ProductCategoryTeamsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductCategoryTeamRoutingModule {}
