import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoreAccountTeamsComponent } from './storeAccountTeams.component';

const routes: Routes = [
    {
        path: '',
        component: StoreAccountTeamsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StoreAccountTeamRoutingModule {}
