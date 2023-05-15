import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductTeamsComponent } from './productTeams.component';

const routes: Routes = [
    {
        path: '',
        component: ProductTeamsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductTeamRoutingModule {}
