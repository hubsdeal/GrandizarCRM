import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HubTypesComponent } from './hubTypes.component';

const routes: Routes = [
    {
        path: '',
        component: HubTypesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class HubTypeRoutingModule {}
