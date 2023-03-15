import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HubsComponent } from './hubs.component';

const routes: Routes = [
    {
        path: '',
        component: HubsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubRoutingModule {}
