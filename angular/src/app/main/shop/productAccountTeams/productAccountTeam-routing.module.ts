import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductAccountTeamsComponent } from './productAccountTeams.component';

const routes: Routes = [
    {
        path: '',
        component: ProductAccountTeamsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ProductAccountTeamRoutingModule {}
